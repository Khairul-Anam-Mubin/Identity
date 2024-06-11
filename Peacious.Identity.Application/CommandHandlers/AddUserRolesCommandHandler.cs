using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;

namespace Peacious.Identity.Application.CommandHandlers;

public class AddUserRolesCommandHandler : ICommandHandler<AddUserRolesCommand>
{
    public async Task<IResult> Handle(AddUserRolesCommand request, CancellationToken cancellationToken)
    {
        return Result.Success("Roles added");
    }
}
