using Peacious.Framework.PermissionAuthorization;

namespace Peacious.Identity.Application.Services;

public class PermissionProvider : IPermissionProvider
{
    public bool HasPermission(string permission)
    {
        return HasPermissionAsync(permission).Result;
    }

    public async Task<bool> HasPermissionAsync(string permission)
    {
        return await Task.FromResult(true);
    }
}
