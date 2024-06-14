using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peacious.Framework;
using Peacious.Framework.DDD;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Migrations;
using Peacious.Framework.ServiceInstaller;
using Peacious.Identity.Domain.Repositories;
using Peacious.Identity.Infrastructure.Migrations;
using Peacious.Identity.Infrastructure.Repositories;

namespace Peacious.Identity.Infrastructure;

public class IdentityInfrastructureInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMongoDb();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IClientRepository, ClientRepository>();
        services.AddKeyedTransient<IMigrationJob, ClientMigrationJob>("ClientMigrationJob");
    }
}
