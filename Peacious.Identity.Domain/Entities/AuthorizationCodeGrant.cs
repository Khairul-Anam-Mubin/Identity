using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.Security;
using System.Security.Cryptography;

namespace Peacious.Identity.Domain.Entities;

public class AuthorizationCodeGrant : Entity, IRepositoryItem
{
    public string UserId { get; private set; }
    public string ClientId { get; private set; }
    public string Code { get; private set; }
    public string? Scope { get; private set; }
    public string? CodeChallange { get; private set; }
    public string? CodeChallangeMethod { get; private set; }
    public DateTime ExpireAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsUsed { get; private set; }

    private AuthorizationCodeGrant(string userId, string clientId, string code, string? scope, DateTime expireAt, string? codeChallange, string? codeChallangeMethod) 
        : base(GetId(clientId, code))
    {
        UserId = userId;
        ClientId = clientId;
        Code = code;
        Scope = scope;
        ExpireAt = expireAt;
        CodeChallange = codeChallange;
        CodeChallangeMethod = codeChallangeMethod;
        CreatedAt = DateTime.UtcNow;
        IsUsed = false;
    }

    public static AuthorizationCodeGrant Create(string userId, string clientId, string? scope, DateTime expireAt, string? codeChallange, string? codeChallangeMethod, int codeLength = 16)
    {
        var code = RandomNumberGenerator.GetHexString(codeLength);

        return new AuthorizationCodeGrant(userId, clientId, code, scope, expireAt, codeChallange, codeChallangeMethod);
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

    public static string GetId(string clientId, string code)
    {
        return CheckSumGenerator.GetCheckSum($"{clientId}-{code}");
    }
}
