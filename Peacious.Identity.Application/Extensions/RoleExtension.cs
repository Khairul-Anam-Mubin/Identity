using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Domain.Entities;
using System.Security.Claims;

namespace Peacious.Identity.Application.Extensions;

public static class RoleExtension
{
    public static Claim ToClaim(this Role role)
    {
        return new Claim(ClaimType.Role, role.Name);
    }
}
