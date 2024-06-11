using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;

namespace Peacious.Identity.Application.CommandHandlers;

public class AddUserPermissionsCommandHandler : ICommandHandler<AddUserPermissionsCommand>
{
    public async Task<IResult> Handle(AddUserPermissionsCommand request, CancellationToken cancellationToken)
    {
        return Result.Success("User permissions added");
    }
}
