using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class AddUserPermissionsCommandHandler(
    IPermissionRepository permissionRepository) : ICommandHandler<AddUserPermissionsCommand>
{
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task<IResult> Handle(AddUserPermissionsCommand command, CancellationToken cancellationToken)
    {
        var userPermissions = new List<UserPermission>();

        command.PermissionIds.ForEach(permissionId =>
        {
            userPermissions.Add(UserPermission.Create(command.UserId, permissionId));
        });

        await _permissionRepository.SaveUserPermissionsAsync(userPermissions.ToArray());

        return Result.Success("CurrentUser permissions added");
    }
}
