using Peacious.Framework.PermissionAuthorization;
using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.Services;

public class PermissionProvider(
    IUserScopeContext userScopeContext,
    IPermissionRepository permissionRepository) : IPermissionProvider
{
    private readonly IUserScopeContext _userScopeContext = userScopeContext;
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public bool HasPermission(string permission)
    {
        return HasPermissionAsync(permission).Result;
    }

    public async Task<bool> HasPermissionAsync(string permission)
    {
        if (_userScopeContext.User == UserIdentity.Empty || string.IsNullOrEmpty(_userScopeContext.User.Id))
        {
            return false;
        }

        if (_userScopeContext.GetClaim(ClaimType.Source)?.Value == GrantType.AuthorizationCode)
        {
            var scope = _userScopeContext.GetClaim(ClaimType.Scope)?.Value;

            if (string.IsNullOrEmpty(scope))
            {
                return false;
            }

            var scopeList = scope.Split(' ').ToList();

            return scopeList.Contains(permission); // get the permissions from scopeList and check the permission exists on it or not
        }

        // todo : improvement on db calls and caching redis
        var permissions =
            await _permissionRepository.GetUserPermissionsAsync(_userScopeContext.User.Id);

        var hasPermission = permissions.Exists(p => p.Title == permission);

        return hasPermission;
    }
}
