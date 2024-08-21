using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class RemoveChildPermissionsCommandHandler(
IPermissionRepository permissionRepository) : ICommandHandler<RemoveChildPermissionsCommand>
{
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task<IResult> Handle(RemoveChildPermissionsCommand command, CancellationToken cancellationToken)
    {
        await _permissionRepository.RemovePermissionDepdenciesAsync(
            command.PermissionId, command.ChildPermissionIds);

        return Result.Success();
    }
}
