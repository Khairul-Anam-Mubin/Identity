using Peacious.Framework.DDD;

namespace Peacious.Identity.Domain.Entities;

public class UserPermission : Entity
{
    public string PermissionId { get; private set; }
    public string UserId { get; private set; }

    private UserPermission(string userId, string permissionId) 
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        PermissionId = permissionId;
    }
}
