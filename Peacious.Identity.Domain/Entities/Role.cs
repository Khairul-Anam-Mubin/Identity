using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;
using System.Security.Claims;

namespace Peacious.Identity.Domain.Entities;

public class Role : Entity, IRepositoryItem
{
    public string Name { get; private set; }

    private Role(string name) : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }

    public Claim ToClaim()
    {
        return new Claim("role", Name);
    }

    public static Role Create(string name)
    {
        return new Role(name);
    }
}
