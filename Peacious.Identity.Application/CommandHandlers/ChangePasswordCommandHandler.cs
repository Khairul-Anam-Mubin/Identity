using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;

namespace Peacious.Identity.Application.CommandHandlers;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    public async Task<IResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return Result.Success("Password Changed");
    }
}
