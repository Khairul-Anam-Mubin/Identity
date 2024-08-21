using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
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
        var permissionDependencies = new List<PermissionDependency>();

        command.ChildPermissionIds.ForEach(childPermissionId =>
        {
            var permisisonDependency = PermissionDependency.Create(childPermissionId, command.PermissionId);
            permissionDependencies.Add(permisisonDependency);
        });

        await _permissionRepository.AddPermissionDependenciesAsync(permissionDependencies);
        
        return Result.Success();
    }
}
