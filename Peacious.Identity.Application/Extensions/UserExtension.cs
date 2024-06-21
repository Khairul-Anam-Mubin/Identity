using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Domain.Entities;
using System.Security.Claims;

namespace Peacious.Identity.Application.Extensions;

public static class UserExtension
{
    public static List<Claim> ToClaims(this User user)
    {
        return new List<Claim>
        {
            new Claim(ClaimType.UserId, user.Id),
            new Claim(ClaimType.UserName, user.UserName),
            new Claim(ClaimType.FirstName, user.Name.FirstName),
            new Claim(ClaimType.LastName, user.Name.LastName),
        };
    }
}
