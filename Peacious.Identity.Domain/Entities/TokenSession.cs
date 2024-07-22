using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.Results;
using Peacious.Framework.Results.Errors;
using Peacious.Identity.Domain.Errors;
using System.Text;

namespace Peacious.Identity.Domain.Entities;

public class TokenSession : Entity, IRepositoryItem
{
    public string ClientId { get; private set; }
    public string Scope { get; private set; }
    public string Role { get; private set; }
    public DateTime ExpireAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime RefreshedAt { get; private set; }
    public bool IsRefreshed { get; private set; }
    public string UserId { get; private set; }

    private TokenSession(string userId, string clientId, string scope, string role, DateTime expireAt) 
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

    public static TokenSession Create(string userId, string clientId, string scope, string role, DateTime expireAt)
    {
        return new TokenSession(userId, clientId, scope, role, expireAt);
    }

    public static TokenSession Create(string userId, string clientId, List<Permission> permissions, List<Role> roles, DateTime expireAt)
    {
        var scopeBuilder = new StringBuilder();

        for (int i = 0; i < permissions.Count; i++)
        {
            scopeBuilder.Append(permissions[i]);

            if (i + 1 != permissions.Count)
            {
                scopeBuilder.Append(" ");
            }
        }

        var scope = scopeBuilder.ToString();

        var roleBuilder = new StringBuilder();

        for (int i = 0; i < roles.Count; i++)
        {
            roleBuilder.Append(roles[i]);

            if (i + 1 != roles.Count)
            {
                roleBuilder.Append(" ");
            }
        }

        var role = roleBuilder.ToString();

        return new TokenSession(userId, clientId, scope, role, expireAt);
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
}
