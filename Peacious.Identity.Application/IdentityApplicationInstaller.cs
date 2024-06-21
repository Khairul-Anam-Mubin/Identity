using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peacious.Framework;
using Peacious.Framework.CQRS;
using Peacious.Framework.DDD;
using Peacious.Framework.Mediators;
using Peacious.Framework.ServiceInstaller;
using Peacious.Identity.Application.Services;

namespace Peacious.Identity.Application;

public class IdentityApplicationInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediator(AssemblyCache.Instance.GetAddedAssemblies());
        services.AddCQRS();
        services.AddDDD();
        services.AddTransient<ITokenService, TokenService>();
    }
}
