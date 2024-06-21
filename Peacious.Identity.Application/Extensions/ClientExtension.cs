using Peacious.Identity.Contracts.Constants;
using Peacious.Identity.Domain.Entities;
using System.Security.Claims;

namespace Peacious.Identity.Application.Extensions;

public static class ClientExtension
{
    public static List<Claim> ToClaims(this Client client)
    {
        return new List<Claim>
        {
            new Claim(ClaimType.ClientId, client.Id),
            new Claim(ClaimType.ClientName, client.Name)
        };
    }
}
