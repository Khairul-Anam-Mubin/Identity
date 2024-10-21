using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;

namespace Peacious.Identity.Application.CommandHandlers;

public class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
{
    public async Task<IResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        return Result.Success("Address verified");
    }
}
