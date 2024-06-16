using Peacious.Framework.DDD;

namespace Peacious.Identity.Domain.Events;

public class UserCreatedDomainEvent : DomainEvent
{
    public string UserName { get; private set; }

    internal UserCreatedDomainEvent(string id, string userName) : base(id)
    {
        UserName = userName;
    }
}
