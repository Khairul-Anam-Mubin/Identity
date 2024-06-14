using MediatR;
using Peacious.Framework.DDD;
using Peacious.Framework.EDD;
using Peacious.Identity.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacious.Identity.Application.DomainEventHandlers;

public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"UserName: {notification.Id} Event Executing... ");
        // todo : send email
    }
}
