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

public class RoleRepository : RepositoryBaseWrapper<Role>, IRoleRepository
{
    public RoleRepository(IConfiguration configuration, IDbContextFactory dbContextFactory, IEventExecutor eventExecutor)
        : base(configuration.TryGetConfig<DatabaseInfo>("MongoDbConfig"), dbContextFactory.GetDbContext(Context.Mongo), eventExecutor)
    { }

    public async Task<List<Role>> GetUserRolesAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<UserRole>();

        var userIdFilter = filterBuilder.Eq(role => role.UserId, userId);

        var userRoles = await DbContext.GetManyAsync<UserRole>(DatabaseInfo, userIdFilter);

        var roleIds = userRoles.Select(role => role.Id).ToList();

        return await GetManyByIdsAsync(roleIds);
    }
}
