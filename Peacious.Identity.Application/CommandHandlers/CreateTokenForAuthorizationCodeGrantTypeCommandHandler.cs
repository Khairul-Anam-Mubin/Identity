using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Contracts.Models;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreateTokenForAuthorizationCodeGrantTypeCommandHandler(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    IAuthorizationCodeGrantRepository authorizationCodeGrantRepository,
    ITokenService tokenService)
    : ICommandHandler<CreateTokenForAuthorizationCodeGrantTypeCommand, TokenResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IAuthorizationCodeGrantRepository _authorizationCodeGrantRepository = authorizationCodeGrantRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;

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

        if (authorizationCodeGrant.HasCodeChallange())
        {
            // verify code challange
        }

        if (authorizationCodeGrant.IsUsed)
        {
            return OAuthError.UnauthorizedClient.Result<TokenResponse>();
        }

        if (authorizationCodeGrant.IsExpired())
        {
            // codegrant expired
        }

        authorizationCodeGrant.SetAsUsed();

        await _authorizationCodeGrantRepository.SaveAsync(authorizationCodeGrant);

        var user = await _userRepository.GetByIdAsync(authorizationCodeGrant.UserId);

        if (user is null)
        {
            return OAuthError.ServerError.Result<TokenResponse>();
        }

        var accessToken = _tokenService.CreateUserAccessToken(user, client, authorizationCodeGrant.Scope);
        var refreshTokenCreatedResult = await _tokenService.CreateUserRefreshTokenAsync(user, client);

        if (refreshTokenCreatedResult.IsFailure)
        {
            return OAuthError.ServerError.Result<TokenResponse>();
        }
        
        var tokenResponse = new TokenResponse(
            TokenType.Bearer,
            accessToken,
            _tokenService.AccessTokenExpirationTimeInSecond(),
            refreshTokenCreatedResult.Value,
            authorizationCodeGrant.Scope);

        return Result.Success(tokenResponse);
    }
}
