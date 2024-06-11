using Peacious.Framework.DDD;
using Peacious.Identity.Domain.ValueObjects;

namespace Peacious.Identity.Domain.Entities;

public class Client : Entity
{
    public string Name { get; private set; }
    public string WebSite { get; private set; }
    public string LogoUrl { get; private set; }
    public string RedirectUri { get; private set; }
    public Secret Secret { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public string UserId { get; private set; }

    private Client(string userId, string name, Secret secret, string website, string logoUrl, string redirectUri) 
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        Name = name;
        Secret = secret;
        WebSite = website;
        LogoUrl = logoUrl;
        RedirectUri = redirectUri;
        IsActive = true;
        IsDeleted = true;
    }

    public static Client Create(string userId, string name, string secret, string website, string logoUrl, string redirectUri)
    {
        return new Client(userId, name, Secret.Create(secret), website, logoUrl, redirectUri);
    }

    public void Revoke()
    {
        IsActive = false;
    }
}
