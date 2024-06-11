using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peacious.Framework;
using Peacious.Framework.DDD;
using Peacious.Framework.ORM;
using Peacious.Framework.ServiceInstaller;
using Peacious.Identity.Domain.Repositories;
using Peacious.Identity.Infrastructure.Repositories;

namespace Peacious.Identity.Infrastructure;

public class IdentityInfrastructureInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDDD(AssemblyCache.Instance.GetAddedAssemblies());
        services.AddMongoDb();
        services.AddTransient<IUserRepository, UserRepository>();
    }
}
