using Microsoft.Extensions.Configuration;
using Peacious.Framework.DDD;
using Peacious.Framework.EDD;
using Peacious.Framework.ORM.Enums;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.ORM;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;
using Peacious.Framework.Extensions;

namespace Peacious.Identity.Infrastructure.Repositories;

public class TokenSessionRepository : RepositoryBaseWrapper<TokenSession> ,ITokenSessionRepository
{
    public TokenSessionRepository(IConfiguration configuration, IDbContextFactory dbContextFactory, IEventExecutor eventExecutor)
    : base(configuration.TryGetConfig<DatabaseInfo>("MongoDbConfig"), dbContextFactory.GetDbContext(Context.Mongo), eventExecutor)
    { }
}
