using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.WalletAgg;
using Users.Infrastructure.Persistence.Context;

namespace Users.Infrastructure.Persistence.Repository
{
    internal class WalletRepository : GenericRepository<Wallet, int>, IWalletRepository
    {
        private readonly UserContext _userContext;

        public WalletRepository(UserContext context) : base(context)
        {

            _userContext = context;
        }

        public async Task<Wallet> GetWalletByUserIdAsync(int userId)
        {
            return await _userContext.Wallets.SingleOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
