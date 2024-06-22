using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.Models;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreateTokenForRefreshTokenGrantTypeCommandHandler(
    IClientRepository clientRepository,
    ITokenSessionRepository tokenSessionRepository,
    ITokenService tokenService)
    : ICommandHandler<CreateTokenForRefreshTokenGrantTypeCommand, TokenResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ITokenSessionRepository _tokenSessionRepository = tokenSessionRepository;

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

        var clientId = claims.FirstOrDefault(claim => claim.Type == ClaimType.ClientId)?.Value;

        if (string.IsNullOrEmpty(clientId))
        {
            return OAuthError.InvalidToken("client_id missing while validating the token.").Result<TokenResponse>();
        }

        if (!clientId.Equals(command.ClientId))
        {
            return OAuthError.InvalidClient(clientId).Result<TokenResponse>();
        }

        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client is null)
        {
            return OAuthError.InvalidClient(clientId).Result<TokenResponse>();
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

        await _tokenSessionRepository.SaveAsync(tokenSession);

        var refreshToken = await _tokenService.CreateUserRefreshTokenAsync(tokenSession, claims);

        var accessToken = _tokenService.CreateUserAccessToken(tokenSession, claims);

        var tokenResponse = new TokenResponse(
            TokenType.Bearer,
            accessToken,
            _tokenService.AccessTokenExpirationTimeInSecond(),
            refreshToken,
            tokenSession.Scope);

        return Result.Success(tokenResponse);
    }
}
