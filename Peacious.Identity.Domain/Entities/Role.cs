using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;

namespace Peacious.Identity.Domain.Entities;

public class Role : Entity, IRepositoryItem
{
    public string Name { get; private set; }

    private Role(string name) : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }
}
