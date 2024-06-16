using Peacious.Framework.DDD;
using Peacious.Identity.Domain.Events;

namespace Peacious.Identity.Application.DomainEventHandlers;

public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"UserName: {@event.Id} Event Executing... ");
        // todo : send email
    }
}
