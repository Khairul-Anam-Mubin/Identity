using Peacious.Framework.CQRS;
using Peacious.Identity.Domain.Entities;

namespace Peacious.Identity.Application.Queries;

public class UserPermissionsQuery : IQuery<List<Permission>>
{
    public string UserId { get; set; }
}
