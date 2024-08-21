﻿using Peacious.Framework.ORM.Interfaces;
using Peacious.Identity.Domain.Entities;

namespace Peacious.Identity.Domain.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<List<Permission>> GetUserPermissionsAsync(string userId);

    Task<List<Permission>> GetClientPermissionsAsync(string clientId);

    Task<bool> AddPermissionDependenciesAsync(List<PermissionDependency> permissionDependencies);

    Task<bool> RemovePermissionDepdenciesAsync(string parentPermissionId, List<string> childPermissionIds);
}
