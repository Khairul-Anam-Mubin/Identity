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

public class CreateTokenForAuthorizationCodeGrantTypeCommandHandler(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITokenSessionRepository tokenSessionRepository,
    IAuthorizationCodeGrantRepository authorizationCodeGrantRepository,
    ITokenService tokenService)
    : ICommandHandler<CreateTokenForAuthorizationCodeGrantTypeCommand, TokenResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IAuthorizationCodeGrantRepository _authorizationCodeGrantRepository = authorizationCodeGrantRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ITokenSessionRepository _tokenSessionRepository = tokenSessionRepository;

    public async Task<IResult<TokenResponse>> Handle(CreateTokenForAuthorizationCodeGrantTypeCommand command, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId);

        if (client is null)
        {
            return OAuthError.InvalidClient(command.ClientId).Result<TokenResponse>();
        }

        if (!client.IsValidRedirectUri(command.RedirectUri))
        {
            return OAuthError.InvalidRequest("redirect_uri").Result<TokenResponse>();
        }

        if (!string.IsNullOrEmpty(command.ClientSecret))
        {
            if (!client.Secret.IsMatch(command.ClientSecret))
            {
                return OAuthError.InvalidCredentials.Result<TokenResponse>();
            }
        }

        var authorizationCodeGrant =
            await _authorizationCodeGrantRepository.GetByIdAsync(AuthorizationCodeGrant.GetId(client.Id, command.Code));

        if (authorizationCodeGrant is null) 
        {
            return OAuthError.UnauthorizedClient.Result<TokenResponse>();
        }

        var verifyCodeChallengeResult = authorizationCodeGrant.VerifyCodeChallenge(command.CodeVerifier);

        if (verifyCodeChallengeResult.IsFailure)
        {
            return verifyCodeChallengeResult.ToResult<TokenResponse>();
        }

        if (authorizationCodeGrant.IsUsed)
        {
            return OAuthError.UnauthorizedClient.Result<TokenResponse>();
        }

        if (authorizationCodeGrant.IsExpired())
        {
            return OAuthError.UnauthorizedClient.Result<TokenResponse>();
        }

        authorizationCodeGrant.SetAsUsed();

        await _authorizationCodeGrantRepository.SaveAsync(authorizationCodeGrant);

        var user = await _userRepository.GetByIdAsync(authorizationCodeGrant.UserId);

        if (user is null)
        {
            return OAuthError.ServerError.Result<TokenResponse>();
        }

        var tokenSession = TokenSession.Create(
            user.Id,
            client.Id,
            authorizationCodeGrant.Scope,
            DateTime.UtcNow.AddSeconds(_tokenService.RefreshTokenExpirationTimeInSecond()),
            GrantType.AuthorizationCode);

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
            new Claim(ClaimType.Scope, tokenSession.Scope ?? string.Empty),
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
            tokenSession.Scope);

        return Result.Success(tokenResponse);
    }
}
