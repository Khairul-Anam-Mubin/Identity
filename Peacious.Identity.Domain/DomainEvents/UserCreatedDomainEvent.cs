using Peacious.Framework.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacious.Identity.Domain.DomainEvents;

public class UserCreatedDomainEvent : DomainEvent
{
    public string UserName { get; private set; }

    internal UserCreatedDomainEvent(string id, string userName) : base(id) 
    {
        UserName = userName;
    }
}
