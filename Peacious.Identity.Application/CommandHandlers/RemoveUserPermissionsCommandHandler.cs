using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class RemoveUserPermissionsCommandHandler(
    IPermissionRepository permissionRepository) : ICommandHandler<RemoveUserPermissionsCommand>
{
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task<IResult> Handle(RemoveUserPermissionsCommand command, CancellationToken cancellationToken)
    {
        await _permissionRepository.RemoveUserPermissionsAsync(command.UserId, command.PermissionIds);

        return Result.Success("CurrentUser permissions removed");
    }
}
