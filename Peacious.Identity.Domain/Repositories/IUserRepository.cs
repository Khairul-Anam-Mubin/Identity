using Peacious.Framework.ORM.Interfaces;
using Peacious.Identity.Domain.Entities;

namespace Peacious.Identity.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByUserNameAsync(string userName);
    Task<bool> IsUserEmailExistAsync(string email);
}
