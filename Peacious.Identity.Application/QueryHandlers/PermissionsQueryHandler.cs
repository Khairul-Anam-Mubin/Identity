using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Queries;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.QueryHandlers;

public class PermissionsQueryHandler(
    IPermissionRepository permissionRepository) : IQueryHandler<PermissionsQuery, List<Permission>>
{
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task<IResult<List<Permission>>> Handle(PermissionsQuery query, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetManyAsync();

        return Result.Success(permissions);
    }
}
