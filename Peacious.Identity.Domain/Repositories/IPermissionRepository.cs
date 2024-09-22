using Peacious.Framework.ORM.Interfaces;
using Peacious.Identity.Domain.Entities;

namespace Peacious.Identity.Domain.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<List<Permission>> GetCustomPermissionsAsync();

    Task<List<Permission>> GetCustomPermissionsByParentPermissionIdAsync(string parentPermissionId);

    Task<List<Permission>> GetCustomPermissionsByParentPermissionIdsAsync(List<string> parentPermissionIds);

    Task<List<Permission>> GetDirectDependentPermissionsAsync(string parentPermissionId);

    Task<bool> IsPermissionExistByTitleAsync(string title);

    Task<List<Permission>> GetUserPermissionsAsync(string userId);

    Task<List<Permission>> GetClientPermissionsAsync(string clientId);

    Task<bool> AddPermissionDependenciesAsync(List<PermissionDependency> permissionDependencies);

    Task<bool> RemovePermissionDepdenciesAsync(string parentPermissionId, List<string> childPermissionIds);

    Task<bool> SaveUserPermissionsAsync(params UserPermission[] userPermissions);

    Task<bool> RemoveUserPermissionsAsync(string userId, List<string> permissionIds);

    Task<List<PermissionDependency>> GetPermissionDependenciesAsync();
}
