using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.User.Agg.IRepository
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        Task <User> GetByMobile(string mobile);
    }
}
