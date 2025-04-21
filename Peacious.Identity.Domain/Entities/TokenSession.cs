using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Domain.Errors;

namespace Peacious.Identity.Domain.Entities;

public class TokenSession : Entity, IRepositoryItem
{
    public string ClientId { get; private set; }
    public string? GrantType { get; private set; }
    public string? Scope { get; private set; }
    public DateTime ExpireAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime RefreshedAt { get; private set; }
    public bool IsRefreshed { get; private set; }
    public string UserId { get; private set; }

    private TokenSession(string userId, string clientId, string? grantType, string? scope, DateTime expireAt) 
        : base(Guid.NewGuid().ToString())
    {
        UserId = userId;
        ClientId = clientId;
        GrantType = grantType;
        Scope = scope;
        ExpireAt = expireAt;
        CreatedAt = DateTime.UtcNow;
        IsRefreshed = false;
    }

    public static TokenSession Create(string userId, string clientId, string? scope, DateTime expireAt, string? grantType = "")
    {
        return new TokenSession(userId, clientId, grantType, scope, expireAt);
    }

    public IResult Refresh()
    {
        if (IsExpired())
        {
            return OAuthError.InvalidToken("Refresh token already expired.").Result();
        }

        if (IsRefreshed)
        {
            return OAuthError.InvalidToken("Already refreshed with this token.").Result();
        }

        IsRefreshed = true;
        RefreshedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public bool IsExpired()
    {
        return ExpireAt < DateTime.UtcNow;
    }

    public string GetRepositoryName()
    {
        return nameof(TokenSession);
    }
}
