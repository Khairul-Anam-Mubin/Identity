using Peacious.Framework.DDD;

namespace Peacious.Identity.Domain.Entities;

public class Verification : Entity
{
    public string UserId { get; private set; }
    public string Identifier { get; private set; }
    public string Identity { get; private set; }
    public string Code { get; private set; }
    public string Type { get; private set; }
    public DateTime ExpireAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsVerified { get; private set; }

    private Verification(string userId, string identifier, string identity, string code, string type, DateTime expireAt) 
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        Identifier = identifier;
        Identity = identity;
        Code = code;
        Type = type;
        ExpireAt = expireAt;
        CreatedAt = DateTime.UtcNow;
        IsVerified = false;
    }
}
