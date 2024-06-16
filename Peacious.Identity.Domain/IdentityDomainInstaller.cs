using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peacious.Framework.ServiceInstaller;
using Peacious.Identity.Domain.Services;

namespace Peacious.Identity.Domain;

public class IdentityDomainInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAccessService, AccessService>();
    }
}
