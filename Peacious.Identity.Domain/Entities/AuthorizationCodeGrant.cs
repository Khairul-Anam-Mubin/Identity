using Peacious.Framework.DDD;
using System.Security.Cryptography;

namespace Peacious.Identity.Domain.Entities;

public class AuthorizationCodeGrant : Entity
{
    public string UserId { get; private set; }
    public string ClientId { get; private set; }
    public string Code { get; private set; }
    public string Scope { get; private set; }
    public string? CodeChallange { get; private set; }
    public string? CodeChallangeMethod { get; private set; }
    public DateTime ExpireAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsUsed { get; private set; }

    private AuthorizationCodeGrant(string userId, string clientId, string code, string scope, DateTime expireAt) 
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        ClientId = clientId;
        Code = code;
        Scope = scope;
        ExpireAt = expireAt;
        CreatedAt = DateTime.UtcNow;
        IsUsed = false;
    }

    public static AuthorizationCodeGrant Create(string userId, string clientId, string scope, DateTime expireAt, int codeLength = 16)
    {
        var code = RandomNumberGenerator.GetHexString(codeLength);

        return new AuthorizationCodeGrant(userId, clientId, code, scope, expireAt);
    }

    public bool HasCodeChallange()
    {
        return string.IsNullOrEmpty(CodeChallange) 
            || string.IsNullOrEmpty(CodeChallangeMethod);
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpireAt;
    }

    public void SetAsUsed()
    {
        IsUsed = true;
    }
}
