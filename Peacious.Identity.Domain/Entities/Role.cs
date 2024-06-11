using Peacious.Framework.DDD;

namespace Peacious.Identity.Domain.Entities;

public class Role : Entity
{
    public string Name { get; private set; }

    private Role(string name) : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }
}
