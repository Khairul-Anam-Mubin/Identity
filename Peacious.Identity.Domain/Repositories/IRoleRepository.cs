using Peacious.Framework.ORM.Interfaces;
using Peacious.Identity.Domain.Entities;

namespace Peacious.Identity.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<List<Role>> GetUserRolesAsync(string userId);
}
