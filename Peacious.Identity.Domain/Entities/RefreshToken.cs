using Peacious.Framework.DDD;

namespace Peacious.Identity.Domain.Entities;

public class RefreshToken : Entity
{
    public string ClientId { get; private set; }
    public string Scope { get; private set; }
    public string Role { get; private set; }
    public DateTime ExpireAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsRefreshed { get; private set; }
    public string UserId { get; private set; }

    private RefreshToken(string userId, string clientId, string scope, string role, DateTime expireAt) 
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        ClientId = clientId;
        Scope = scope;
        Role = role;
        ExpireAt = expireAt;
        CreatedAt = DateTime.UtcNow;
        IsRefreshed = false;
    }
}
