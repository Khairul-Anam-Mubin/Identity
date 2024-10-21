using Peacious.Framework.CQRS;
using Peacious.Framework.IdentityScope;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Extensions;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Domain.Repositories;
using System.Security.Claims;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreateTokenForRefreshTokenGrantTypeCommandHandler(
    IClientRepository clientRepository,
    ITokenSessionRepository tokenSessionRepository,
    IUserRepository userRepository,
    ITokenService tokenService)
    : ICommandHandler<CreateTokenForRefreshTokenGrantTypeCommand, TokenResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ITokenSessionRepository _tokenSessionRepository = tokenSessionRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IResult<TokenResponse>> Handle(CreateTokenForRefreshTokenGrantTypeCommand command, CancellationToken cancellationToken)
    {
        var tokenValidationResult = _tokenService.ValidateToken(command.RefreshToken);

        if (tokenValidationResult.IsFailure)
        {
            return tokenValidationResult.ToResult<TokenResponse>();
        }
        
        var claims = TokenHelper.GetClaims(command.RefreshToken);

        var tokenSessionId = claims.FirstOrDefault(claim => claim.Type == ClaimType.JwtTokenId)?.Value;

        if (string.IsNullOrEmpty(tokenSessionId))
        {
            return OAuthError.InvalidToken("jti missing while validating the token.").Result<TokenResponse>();
        }

        var tokenSession = await _tokenSessionRepository.GetByIdAsync(tokenSessionId);

        if (tokenSession is null)
        {
            return OAuthError.InvalidToken("TokenSession not found").Result<TokenResponse>();
        }

        var tokenSessionRefreshResult = tokenSession.Refresh();

        if (tokenSessionRefreshResult.IsFailure)
        {
            return tokenSessionRefreshResult.ToResult<TokenResponse>();
        }

        var clientId = tokenSession.ClientId;

        if (!clientId.Equals(command.ClientId))
        {
            return OAuthError.InvalidClient(clientId).Result<TokenResponse>();
        }
        
        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client is null)
        {
            return OAuthError.InvalidClient(clientId).Result<TokenResponse>();
        }
        
        var user = await _userRepository.GetByIdAsync(tokenSession.UserId);

        if (user is null)
        {
            return OAuthError.InvalidUser(clientId).Result<TokenResponse>();    
        }

        var newTokenSession = TokenSession.Create(
            tokenSession.UserId,
            tokenSession.ClientId,
            tokenSession.Scope,
            DateTime.UtcNow.AddSeconds(_tokenService.RefreshTokenExpirationTimeInSecond()),
            tokenSession.GrantType);

        await _tokenSessionRepository.SaveAsync(tokenSession);
        await _tokenSessionRepository.SaveAsync(newTokenSession);

        var refreshTokenClaims = new List<Claim>
        {
            new Claim(ClaimType.JwtTokenId, newTokenSession.Id)
        };

        var userClaims = user.ToClaims();
        var clientClaims = client.ToClaims();

        refreshTokenClaims.AddRange(userClaims);
        refreshTokenClaims.AddRange(clientClaims);

        var refreshToken = _tokenService.GenerateRefreshToken(refreshTokenClaims);

        var accessTokenClaims = new List<Claim>
        {
            new Claim(ClaimType.JwtTokenId, Guid.NewGuid().ToString()),
            new Claim(ClaimType.Scope, newTokenSession.Scope ?? string.Empty),
            new Claim(ClaimType.Source, newTokenSession.GrantType ?? string.Empty)
        };

        accessTokenClaims.AddRange(clientClaims);
        accessTokenClaims.AddRange(userClaims);

        var accessToken = _tokenService.GenerateAccessToken(accessTokenClaims);

        var tokenResponse = new TokenResponse(
            TokenType.Bearer,
            accessToken,
            _tokenService.AccessTokenExpirationTimeInSecond(),
            refreshToken,
            tokenSession.Scope);

        return Result.Success(tokenResponse);
    }
}
