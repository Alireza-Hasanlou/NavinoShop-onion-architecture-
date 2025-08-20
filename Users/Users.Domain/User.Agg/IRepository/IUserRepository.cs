using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Domain;

namespace Users.Domain.User.Agg.IRepository
{
    public interface IUserRepository:IGenericRepository<User,int>
    {
    }
}
