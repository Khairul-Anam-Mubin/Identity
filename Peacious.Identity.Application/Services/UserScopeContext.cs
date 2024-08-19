using Peacious.Framework.IdentityScope;
using Peacious.Identity.Contracts.Constants;
using System.Security.Claims;

namespace Peacious.Identity.Application.Services;

public class UserScopeContext(
    IIdentityScopeContext identityScopeContext) : IUserScopeContext
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

            if (string.IsNullOrEmpty(_identityScopeContext.Token))
            {
                return UserIdentity.Empty;
            }

            _user = new UserIdentity(
                _identityScopeContext.GetClaim(ClaimType.UserId)?.Value,
                _identityScopeContext.GetClaim(ClaimType.UserName)?.Value,
                _identityScopeContext.GetClaim(ClaimType.FirstName)?.Value,
                _identityScopeContext.GetClaim(ClaimType.LastName)?.Value);
            
            return _user;
        }
    }

    public string? Token => _identityScopeContext.Token;

    public List<Claim> Claims => _identityScopeContext.Claims;

    public Claim? GetClaim(string claimType)
    {
        return _identityScopeContext.GetClaim(claimType);
    }

    public bool HasClaim(string claimType)
    {
        return _identityScopeContext.HasClaim(claimType);
    }
}