using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class DeletePermissionsCommandHandler(
    IUserScopeContext userScopeContext,
    IPermissionRepository permissionRepository) : ICommandHandler<DeletePermissionsCommand>
{
    private readonly IUserScopeContext _userScopeContext = userScopeContext;
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task<IResult> Handle(DeletePermissionsCommand command, CancellationToken cancellationToken)
    {
        await _permissionRepository.DeleteManyByIdsAsync(command.PermissionIds);

        return Result.Success();
    }
}
