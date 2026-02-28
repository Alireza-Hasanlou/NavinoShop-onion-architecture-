using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using Users.Infrastructure.Persistence.Context;
using Users.Infrastructure.Persistence.EFConfig;

namespace Users.Infrastructure.Persistence.Repository
{
    internal class PermissionRepository : GenericRepository<Permission, int>, IPermissionRepository
    {
        private readonly UserContext _userContext;
        public PermissionRepository(UserContext userContext) : base(userContext)
        {
            _userContext = userContext; 
        }
    }
}
