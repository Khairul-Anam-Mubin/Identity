using Peacious.Framework.CQRS;
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

public class CreateTokenForPasswordGrantTypeCommandHandler(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITokenSessionRepository tokenSessionRepository,
    ITokenService tokenService) : ICommandHandler<CreateTokenForPasswordGrantTypeCommand, TokenResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ITokenSessionRepository _tokenSessionRepository = tokenSessionRepository;

    public async Task<IResult<TokenResponse>> Handle(
        CreateTokenForPasswordGrantTypeCommand command, 
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId);

        if (client is null)
        {
            return OAuthError.InvalidClient(command.ClientId).Result<TokenResponse>();
        }

        var user = await _userRepository.GetUserByUserNameAsync(command.UserName);

        if (user is null)
        {
            return OAuthError.InvalidUser(command.UserName).Result<TokenResponse>();
        }

        if (!user.Password.IsMatch(command.Password))
        {
            return OAuthError.InvalidCredentials.Result<TokenResponse>();
        }

        var tokenSession = TokenSession.Create(
            user.Id,
            client.Id,
            string.Empty,
            DateTime.UtcNow.AddSeconds(_tokenService.RefreshTokenExpirationTimeInSecond()),
            GrantType.Password);

        await _tokenSessionRepository.SaveAsync(tokenSession);

        var refreshTokenClaims = new List<Claim>
        {
            new Claim(ClaimType.JwtTokenId, tokenSession.Id)
        };

        var userClaims = user.ToClaims();
        var clientClaims = client.ToClaims();

        refreshTokenClaims.AddRange(userClaims);
        refreshTokenClaims.AddRange(clientClaims);

        var refreshToken = _tokenService.GenerateRefreshToken(refreshTokenClaims);

        var accessTokenClaims = new List<Claim>
        {
            new Claim(ClaimType.JwtTokenId, Guid.NewGuid().ToString()),
            new Claim(ClaimType.Source, tokenSession.GrantType ?? string.Empty)
        };

        accessTokenClaims.AddRange(clientClaims);
        accessTokenClaims.AddRange(userClaims);

        var accessToken = _tokenService.GenerateAccessToken(accessTokenClaims);

        var tokenResponse = new TokenResponse(
            TokenType.Bearer,
            accessToken, 
            _tokenService.AccessTokenExpirationTimeInSecond(), 
            refreshToken, 
            null);

        return Result.Success(tokenResponse);
    }
}
