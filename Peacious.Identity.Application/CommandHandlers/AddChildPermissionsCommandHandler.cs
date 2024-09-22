using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Application.Commands;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.CommandHandlers;

public class AddChildPermissionsCommandHandler(
    IPermissionRepository permissionRepository) : ICommandHandler<AddChildPermissionsCommand>
{
    private readonly IPermissionRepository _permissionRepository = permissionRepository;
    
    public async Task<IResult> Handle(AddChildPermissionsCommand command, CancellationToken cancellationToken)
    {
        var parentPermission = await _permissionRepository.GetByIdAsync(command.PermissionId);

        if (parentPermission is null )
        {
            return Error.NotFound("Parent permission not found").Result();
        }

        if (parentPermission.IsCustom)
        {
            return Error.NotFound("Custom permission is not allowed to add descendant permission").Result();
        }

        var permissionDependencies = new List<PermissionDependency>();

        command.DependentPermissionIds.ForEach(childPermissionId =>
        {
            var permisisonDependency = PermissionDependency.Create(childPermissionId, command.PermissionId);
            permissionDependencies.Add(permisisonDependency);
        });

        await _permissionRepository.AddPermissionDependenciesAsync(permissionDependencies);
        
        return Result.Success();
    }
}
