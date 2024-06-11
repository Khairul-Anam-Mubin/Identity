using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peacious.Framework;
using Peacious.Framework.CQRS;
using Peacious.Framework.ServiceInstaller;

namespace Peacious.Identity.Application;

public class IdentityApplicationInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCQRS(AssemblyCache.Instance.GetAddedAssemblies());
    }
}
