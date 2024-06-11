using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;

namespace Peacious.Identity.Application.CommandHandlers;

public class ForgetPasswordCommandHandler : ICommandHandler<ForgetPasswordCommand>
{
    public async Task<IResult> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        return Result.Success("Check email for reset password");
    }
}
