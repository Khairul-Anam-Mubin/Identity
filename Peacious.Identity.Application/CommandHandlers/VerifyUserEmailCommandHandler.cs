using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;

namespace Peacious.Identity.Application.CommandHandlers;

public class VerifyUserEmailCommandHandler : ICommandHandler<VerifyUserEmailCommand>
{
    public async Task<IResult> Handle(VerifyUserEmailCommand request, CancellationToken cancellationToken)
    {
        return Result.Success("Address verified");
    }
}
