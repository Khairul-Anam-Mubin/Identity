using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreatePermissionCommandHandler(
    IPermissionRepository permissionRepository) : ICommandHandler<CreatePermissionCommand>
{
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task<IResult> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
    {
        if (await _permissionRepository.IsPermissionExistByTitleAsync(command.Title))
        {
            return Error.Conflict("Permission already exist with the same title").Result();
        }

        var permission = Permission.Create(command.Title, command.IsCustom);

        await _permissionRepository.SaveAsync(permission);

        return Result.Success("Permission Created");
    }
}
