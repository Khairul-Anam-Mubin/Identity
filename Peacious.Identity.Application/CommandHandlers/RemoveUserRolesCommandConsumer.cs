using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;

namespace Peacious.Identity.Application.CommandHandlers;

public class RemoveUserRolesCommandConsumer : ICommandHandler<RemoveUserRolesCommand>
{
    public async Task<IResult> Handle(RemoveUserRolesCommand request, CancellationToken cancellationToken)
    {
        return Result.Success("User roles removed");
    }
}
