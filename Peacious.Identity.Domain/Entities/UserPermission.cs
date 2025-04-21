using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;

namespace Peacious.Identity.Domain.Entities;

public class UserPermission : Entity, IRepositoryItem
{
    public string PermissionId { get; private set; }
    public string UserId { get; private set; }

    private UserPermission(string userId, string permissionId) 
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        PermissionId = permissionId;
    }

    public static UserPermission Create(string userId, string permissionId)
    {
        return new UserPermission(userId, permissionId);
    }

    public string GetRepositoryName()
    {
        return nameof(UserPermission);
    }
}
