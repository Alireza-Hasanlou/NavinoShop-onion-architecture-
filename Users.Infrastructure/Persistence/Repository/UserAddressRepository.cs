using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using Users.Infrastructure.Persistence.Context;
using Utility.Shared.Insfrastructure;

namespace Users.Infrastructure.Persistence.Repository
{
    internal class UserAddressRepository : GenericRepository<UserAddress, int>, IUserAddressRepository
    {
        private readonly UserContext _context;
        public UserAddressRepository(UserContext context) : base(context)
        {
            _context = context;
        }
    }
    
}
