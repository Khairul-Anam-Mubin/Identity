using Microsoft.Extensions.Configuration;
using Peacious.Framework.DDD;
using Peacious.Framework.EDD;
using Peacious.Framework.Extensions;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Builders;
using Peacious.Framework.ORM.Enums;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Infrastructure.Repositories;

public class PermissionRepository : RepositoryBaseWrapper<Permission>, IPermissionRepository
{
    public PermissionRepository(IConfiguration configuration, IDbContextFactory dbContextFactory, IEventExecutor eventExecutor)
        : base(configuration.TryGetConfig<DatabaseInfo>("MongoDbConfig"), dbContextFactory.GetDbContext(Context.Mongo), eventExecutor)
    { }

    public async Task<List<Permission>> GetUserPermissionsAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<UserPermission>();
        
        var userIdFilter = filterBuilder.Eq(permission => permission.UserId, userId);
        
        var userPermissions = await DbContext.GetManyAsync<UserPermission>(DatabaseInfo, userIdFilter);
        
        var userPermissionIds = userPermissions.Select(permission => permission.Id).ToList();
        
        return await GetManyByIdsAsync(userPermissionIds);
    }
}
