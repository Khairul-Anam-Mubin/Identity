namespace Peacious.Identity.Contracts.Models;

public record ClientConfig(
    string ClientName,
    string Website,
    string RedirectUri,
    string UserName,
    string ClientId,
    string ClientSecret);
