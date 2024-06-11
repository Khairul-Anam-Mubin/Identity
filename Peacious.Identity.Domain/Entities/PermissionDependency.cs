using Peacious.Framework.DDD;

namespace Peacious.Identity.Domain.Entities;

public class PermissionDependency : Entity
{
    public string PermissionId { get; private set; }
    public string ParentPermissionId { get; private set; }

    private PermissionDependency(string permissionId, string parentPermissionId) 
        : base(Guid.NewGuid().ToString())
    {
        PermissionId = permissionId;
        ParentPermissionId = parentPermissionId;
    }

    public static PermissionDependency Create(string permissionId, string parentPermissionId)
    {
        return new PermissionDependency(permissionId, parentPermissionId);
    }
}
