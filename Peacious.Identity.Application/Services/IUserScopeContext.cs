using Peacious.Framework.IdentityScope;
using Peacious.Identity.Contracts.Constants;

namespace Peacious.Identity.Application.Services;

public class UserIdentity
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public interface IUserScopeContext
{
    UserIdentity User { get; }
}

public class UserScopeContext(IIdentityScopeContext identityScopeContext) : IUserScopeContext
{
    private readonly IIdentityScopeContext _identityScopeContext = identityScopeContext;

    private UserIdentity? _user;
    public UserIdentity User
    {
        get
        {
            if (_user is not null)
            {
                return _user;
            }

            _user = new UserIdentity
            {
                Id = _identityScopeContext.GetClaim(ClaimType.UserId)?.Value,
                UserName = _identityScopeContext.GetClaim(ClaimType.UserName)?.Value,
                FirstName = _identityScopeContext.GetClaim(ClaimType.FirstName)?.Value,
                LastName = _identityScopeContext.GetClaim(ClaimType.LastName)?.Value,
            };
            
            return _user;
        }
    }
}