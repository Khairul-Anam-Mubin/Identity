using Peacious.Framework.DDD;

namespace Peacious.Identity.Domain.Entities;

public class RolePermission : Entity
{
    public string RoleId { get; private set; }
    public string PermissionId { get; private set; }

    private RolePermission(string roleId, string permissionId) 
        : base(Guid.NewGuid().ToString())
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
}
