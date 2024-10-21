using Peacious.Framework.CQRS;
using Peacious.Framework.Results;
using Peacious.Identity.Application.Queries;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Application.QueryHandlers;

public class UserPermissionsQueryHandler(
    IPermissionRepository permissionRepository) : IQueryHandler<UserPermissionsQuery, List<Permission>>
{
    public async Task<IResult<List<Permission>>> Handle(UserPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions =  
            await permissionRepository.GetUserPermissionsAsync(request.UserId);

        return Result.Success(permissions);
    }
}
