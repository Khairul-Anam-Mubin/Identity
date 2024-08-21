using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Domain.Entities;
using System.Security.Claims;

namespace Peacious.Identity.Application.Extensions;

public static class PermissionExtension
{
    public static Claim ToClaim(this Permission permission)
    {
        return new Claim(ClaimType.Scope, permission.Id);
    }
}
