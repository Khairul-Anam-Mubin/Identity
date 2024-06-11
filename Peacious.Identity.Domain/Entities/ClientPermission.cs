using Peacious.Framework.DDD;

namespace Peacious.Identity.Domain.Entities;

public class ClientPermission : Entity
{
    public string PermissionId { get; private set; }
    public string ClientId { get; private set; }

    private ClientPermission(string clientId, string permissionId) 
        : base(Guid.NewGuid().ToString())
    {
        ClientId = clientId;
        PermissionId = permissionId;
    }

    public static ClientPermission Create(string clientId, string permissionId)
    {
        return new ClientPermission(clientId, permissionId);
    }
}
