using Peacious.Framework.DDD;

namespace Peacious.Identity.Domain.Entities;

public class UserRole : Entity
{
    public string RoleId { get; private set; }
    public string UserId { get; private set; }

    private UserRole(string userId, string roleId) : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        RoleId = roleId;
    }
}
