using Microsoft.EntityFrameworkCore;
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
    internal class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByMobile(string mobile)
        {
            return await _context.Users.SingleOrDefaultAsync(m => m.Mobile == mobile);
        }
    }
}
