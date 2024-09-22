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
        if (_userScopeContext.User == UserIdentity.Empty)
        {
            return false;
        }

        var scope = _userScopeContext.GetClaim(ClaimType.Scope)?.Value;

        if (string.IsNullOrEmpty(scope))
        {
            return false;
        }

        var scopeList = scope.Split(' ').ToList();

        var permissions = 
            await _permissionRepository.GetCustomPermissionsByParentPermissionIdsAsync(scopeList);

        var hasPermission = permissions.Exists(p => p.Title == permission);

        return await Task.FromResult(hasPermission);
    }
}
