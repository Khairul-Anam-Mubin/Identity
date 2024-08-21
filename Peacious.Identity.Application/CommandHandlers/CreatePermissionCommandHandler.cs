using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class CreatePermissionCommandHandler(
    IUserScopeContext userScopeContext,
    IPermissionRepository permissionRepository) : ICommandHandler<CreatePermissionCommand>
{
    private readonly IUserScopeContext _userScopeContext = userScopeContext;
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task<IResult> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
    {
        var userId = _userScopeContext.User.Id;

        var permission = Permission.Create(command.Title, false);

        await _permissionRepository.SaveAsync(permission);

        return Result.Success("Permission Created");
    }
}
