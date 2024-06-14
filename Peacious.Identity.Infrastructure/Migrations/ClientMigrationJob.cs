using Microsoft.Extensions.Configuration;
using Peacious.Framework.Extensions;
using Peacious.Framework.ORM.Migrations;
using Peacious.Identity.Contracts;
using Peacious.Identity.Domain.Entities;
using Peacious.Identity.Domain.Repositories;

namespace Peacious.Identity.Infrastructure.Migrations;

public class ClientMigrationJob(IConfiguration configuration, IClientRepository clientRepository) : IMigrationJob
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task MigrateAsync()
    {
        var clientConfigs = _configuration.TryGetConfig<List<ClientConfig>>("PeaciousClients");
        
        var clients = new List<Client>();
        
        clientConfigs.ForEach(clientConfig =>
        {
            var client = Client.Create(
                clientConfig.UserName,
                clientConfig.ClientName,
                clientConfig.ClientId,
                clientConfig.ClientSecret,
                clientConfig.RedirectUri,
                clientConfig.Website,
                null);

            clients.Add(client);
        });

        await _clientRepository.SaveAsync(clients);
    }
}
