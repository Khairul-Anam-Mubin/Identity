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

public class UserRepository : RepositoryBaseWrapper<User>, IUserRepository
{
    public UserRepository(IConfiguration configuration, IDbContextFactory dbContextFactory, IEventExecutor eventExecutor) 
        : base(configuration.TryGetConfig<DatabaseInfo>("MongoDbConfig"), dbContextFactory.GetDbContext(Context.Mongo), eventExecutor)
    {}

    public async Task<bool> IsUserEmailExistAsync(string email)
    {
        var filterBuilder = new FilterBuilder<User>();

        var userNameCondition = filterBuilder.Eq(user => user.Email.Address, email);
        var domainNameCondition = filterBuilder.Eq(user => user.Email.Address, email);

        var andCondition = filterBuilder.And(userNameCondition, domainNameCondition);

        return await DbContext.CountAsync<User>(DatabaseInfo, andCondition) > 0;
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        var filterBuilder = new FilterBuilder<User>();

        var userNameCondition = filterBuilder.Eq(user => user.UserName, userName);

        return await DbContext.GetOneAsync<User>(DatabaseInfo, userNameCondition);
    }
}
