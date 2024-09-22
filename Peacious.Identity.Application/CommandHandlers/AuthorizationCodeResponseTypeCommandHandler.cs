using Microsoft.Extensions.Configuration;
using Peacious.Framework.CQRS;
using Peacious.Framework.Extensions;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Contracts.DTOs;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Errors;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class AuthorizationCodeResponseTypeCommandHandler(
    IUserScopeContext userScopeContext,
    IClientRepository clientRepository,
    IPermissionRepository permissionRepository,
    IAuthorizationCodeGrantRepository authorizationCodeGrantRepository,
    IConfiguration configuration) : ICommandHandler<AuthorizationCodeResponseTypeCommand, AuthorizationResponse>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IPermissionRepository _permissionRepository = permissionRepository;
    private readonly IConfiguration _configuration = configuration;
    private readonly IAuthorizationCodeGrantRepository _authorizationCodeGrantRepository = authorizationCodeGrantRepository;
    public async Task<IResult<AuthorizationResponse>> Handle(AuthorizationCodeResponseTypeCommand command, CancellationToken cancellationToken)
    {
        var userId = userScopeContext.User.Id;

        var client = await _clientRepository.GetByIdAsync(command.ClientId);

        if (client is null)
        {
            return OAuthError.InvalidClient(command.ClientId).Result<AuthorizationResponse>();
        }

        if (!client.IsValidRedirectUri(command.RedirectUri))
        {
            return OAuthError.InvalidRequest("redirect_uri").Result<AuthorizationResponse>();
        }

        if (!string.IsNullOrEmpty(command.Scope))
        {
            var permissions = 
                await _permissionRepository.GetUserPermissionsAsync(userId);

            var scopes = command.Scope.Split(' ');

            foreach (var scope in scopes)
            {
                var permission = permissions.FirstOrDefault(x => x.Title == scope);

                if (permission is null)
                {
                    return OAuthError.InvalidScope.Result<AuthorizationResponse>();
                }
            }
        }

        var expirationTimeInSecond = _configuration.TryGetConfig<int>("OAuth2Config:AuthorizationCodeExpirationTimeInSecond");
        var authorizationCodeLength = _configuration.TryGetConfig<int>("OAuth2Config:AuthorizationCodeLength");

        var authorizationCodeGrant = AuthorizationCodeGrant.Create(
            userId,
            client.Id,
            command.Scope,
            DateTime.UtcNow.AddSeconds(expirationTimeInSecond),
            command.CodeChallenge,
            command.CodeChallengeMethod,
            authorizationCodeLength
            );

        await _authorizationCodeGrantRepository.SaveAsync(authorizationCodeGrant);

        var authorizationResponse = new AuthorizationResponse
        {
            Code = authorizationCodeGrant.Code
        };

        return Result.Success(authorizationResponse);
    }
}
