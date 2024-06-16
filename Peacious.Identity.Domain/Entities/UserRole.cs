using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;

namespace Peacious.Identity.Domain.Entities;

public class UserRole : Entity, IRepositoryItem
{
    public string RoleId { get; private set; }
    public string UserId { get; private set; }

    private UserRole(string userId, string roleId) : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        RoleId = roleId;
    }
}
