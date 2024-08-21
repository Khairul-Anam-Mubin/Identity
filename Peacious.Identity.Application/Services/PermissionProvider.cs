using Peacious.Framework.PermissionAuthorization;
using Peacious.Identity.Contracts.Constants;

namespace Peacious.Identity.Application.Services;

public class PermissionProvider(
    IUserScopeContext userScopeContext) : IPermissionProvider
{
    private readonly IUserScopeContext _userScopeContext = userScopeContext;

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

        var hasPermission = scope.Split(' ').Contains(permission);

        return await Task.FromResult(hasPermission);
    }
}
