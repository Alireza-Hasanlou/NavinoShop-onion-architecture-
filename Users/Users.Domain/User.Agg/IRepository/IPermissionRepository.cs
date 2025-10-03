using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.User.Agg.IRepository
{
    public interface IPermissionRepository:IGenericRepository<Permission,int>
    {
    }
}
