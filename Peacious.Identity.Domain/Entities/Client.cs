using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.Security;
using Peacious.Identity.Domain.ValueObjects;

namespace Peacious.Identity.Domain.Entities;

public class Client : Entity, IRepositoryItem
{
    public string Name { get; private set; }
    public string WebSite { get; private set; }
    public string? LogoUrl { get; private set; }
    public string RedirectUri { get; private set; }
    public Secret Secret { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public string UserName { get; private set; }

    private Client(string userName, string clientName, string clientId, Secret secret, string redirectUri, string website, string? logoUrl) 
        : base(clientId)
    {
        UserName = userName;
        Name = clientName;
        Secret = secret;
        WebSite = website;
        LogoUrl = logoUrl;
        RedirectUri = redirectUri;
        IsActive = true;
        IsDeleted = false;
    }

    public static Client Create(string userName, string clientName, string clientId, string clientSecret, string redirectUri, string website, string? logoUrl)
    {
        return new Client(userName, clientName, clientId, Secret.Create(clientSecret), redirectUri, website, logoUrl);
    }

    public static string CreateClientId(string userName, string clientName)
    {
        return CheckSumGenerator.GetCheckSum($"{userName}-{clientName}");
    }

    public void Revoke()
    {
        IsActive = false;
    }
}
