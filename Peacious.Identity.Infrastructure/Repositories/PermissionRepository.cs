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

    public async Task<bool> AddPermissionDependenciesAsync(List<PermissionDependency> permissionDependencies)
    {
        return await DbContext.SaveManyAsync(DatabaseInfo, permissionDependencies);
    }

    public async Task<bool> RemovePermissionDepdenciesAsync(string parentPermissionId, List<string> childPermissionIds)
    {
        var filterBuilder = new FilterBuilder<PermissionDependency>();

        var parentPermissionFilter = filterBuilder.Eq(x => x.ParentPermissionId, parentPermissionId);
        var childPermissionFilter = filterBuilder.In(x => x.PermissionId, childPermissionIds);

        var filter = filterBuilder.And(parentPermissionFilter, childPermissionFilter);

        return await DbContext.DeleteManyAsync<PermissionDependency>(DatabaseInfo, filter);
    }

    public async Task<List<Permission>> GetClientPermissionsAsync(string clientId)
    {
        var filterBuilder = new FilterBuilder<ClientPermission>();

        var clientIdFilter = filterBuilder.Eq(permission => permission.ClientId, clientId);

        var clientPermissions = await DbContext.GetManyAsync<UserPermission>(DatabaseInfo, clientIdFilter);

        var clientPermissionIds = clientPermissions.Select(permission => permission.Id).ToList();

        return await GetManyByIdsAsync(clientPermissionIds);
    }

    public async Task<List<Permission>> GetUserPermissionsAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<UserPermission>();
        
        var userIdFilter = filterBuilder.Eq(permission => permission.UserId, userId);
        
        var userPermissions = await DbContext.GetManyAsync<UserPermission>(DatabaseInfo, userIdFilter);
        
        var userPermissionIds = userPermissions.Select(permission => permission.Id).ToList();
        
        return await GetManyByIdsAsync(userPermissionIds);
    }
}
