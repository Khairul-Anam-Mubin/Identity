using Peacious.Framework.ORM.Interfaces;
using Peacious.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacious.Identity.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByUserNameAsync(string userName);
    Task<bool> IsUserEmailExistAsync(string email);
}
