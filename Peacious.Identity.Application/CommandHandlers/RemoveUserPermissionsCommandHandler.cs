using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;

namespace Peacious.Identity.Application.CommandHandlers;

public class RemoveUserPermissionsCommandHandler : ICommandHandler<RemoveUserPermissionsCommand>
{
    public async Task<IResult> Handle(RemoveUserPermissionsCommand request, CancellationToken cancellationToken)
    {
        return Result.Success("User permissions removed");
    }
}
