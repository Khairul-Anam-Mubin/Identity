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
        
        var userPermissionIds = userPermissions.Select(permission => permission.PermissionId).ToList();
        
        return await GetManyByIdsAsync(userPermissionIds);
    }

    public async Task<bool> IsPermissionExistByTitleAsync(string title)
    {
        var filterBuilder = new FilterBuilder<Permission>();

        var titleFilter = filterBuilder.Eq(permission => permission.Title, title);

        return await DbContext.GetOneAsync<Permission>(DatabaseInfo, titleFilter) is not null;
    }

    public async Task<bool> SaveUserPermissionsAsync(params UserPermission[] userPermissions)
    {
        return await DbContext.SaveManyAsync(DatabaseInfo, userPermissions.ToList());
    }

    public async Task<bool> RemoveUserPermissionsAsync(string userId, List<string> permissionIds)
    {
        var filterBuilder = new FilterBuilder<UserPermission>();

        var userIdFilter = filterBuilder.Eq(x => x.UserId, userId);
        var permissionIdsFilter = filterBuilder.In(x => x.PermissionId, permissionIds);

        var filter = filterBuilder.And(userIdFilter, permissionIdsFilter);

        return await DbContext.DeleteManyAsync<UserPermission>(DatabaseInfo, filter);
    }
}
