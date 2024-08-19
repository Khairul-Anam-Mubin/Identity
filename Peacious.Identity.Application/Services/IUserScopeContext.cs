using Peacious.Framework.IdentityScope;

namespace Peacious.Identity.Application.Services;

public record UserIdentity(
    string? Id, 
    string? UserName, 
    string? FirstName, 
    string? LastName)
{
    public static UserIdentity Empty = new(null, null, null, null);
}

public interface IUserScopeContext : IIdentityContext
{
    UserIdentity User { get; }
}
