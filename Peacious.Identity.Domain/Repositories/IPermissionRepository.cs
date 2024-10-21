using Peacious.Framework.ORM.Interfaces;
using Peacious.Identity.Domain.Entities;

namespace Peacious.Identity.Domain.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<bool> IsPermissionExistByTitleAsync(string title);

    Task<List<Permission>> GetUserPermissionsAsync(string userId);

    Task<List<Permission>> GetClientPermissionsAsync(string clientId);
    
    Task<bool> SaveUserPermissionsAsync(params UserPermission[] userPermissions);

    Task<bool> RemoveUserPermissionsAsync(string userId, List<string> permissionIds);
}
