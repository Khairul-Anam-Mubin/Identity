using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peacious.Framework.IdentityScope;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Migrations;
using Peacious.Framework.PermissionAuthorization;
using Peacious.Framework.ServiceInstaller;
using Peacious.Identity.Application.Services;
using Peacious.Identity.Domain.Repositories;
using Peacious.Identity.Infrastructure.CommandAdpters;
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
        services.AddIdentityScopeContext();
        services.AddPermissionAuthorization(typeof(PermissionProvider));
        services.AddScoped<IUserScopeContext, UserScopeContext>();

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IClientRepository, ClientRepository>();
        services.AddTransient<IPermissionRepository, PermissionRepository>();
        services.AddTransient<ITokenSessionRepository, TokenSessionRepository>();
        services.AddTransient<IAuthorizationCodeGrantRepository, AuthorizationCodeGrantRepository>();

        services.AddKeyedTransient<IMigrationJob, ClientMigrationJob>(nameof(ClientMigrationJob));
        services.AddKeyedTransient<IMigrationJob, PermissionMigrationJob>(nameof(PermissionMigrationJob));
        //services.AddSingleton(configuration.TryGetConfig<TokenConfig>("TokenConfig"));

        services.AddSingleton(
            new TokenRequestToAuthorizationCodeGrantTypeCommandConverter()
            .SetNext(new TokenRequestToClientCredentialsGrantTypeCommandConverter()
            .SetNext(new TokenRequestToPasswordGrantTypeCommandConverter()
            .SetNext(new TokenRequestToRefreshTokenGrantTypeCommandConverter()
        ))));

        services.AddSingleton<IAuthorizationRequestToCommandConverter>(
            new AuthorizationRequestToAuthorizationCodeResponseTypeCommandConverter()
        );
    }
}
