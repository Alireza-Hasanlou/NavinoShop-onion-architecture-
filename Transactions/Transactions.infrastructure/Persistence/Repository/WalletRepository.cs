using Financial.Domain.WalletAgg;
using Financial.infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Financial.infrastructure.Persistence.Repository
{
    internal class WalletRepository : GenericRepository<Wallet, int>, IWalletRepository
    {
        private readonly FinancialContext _financialContext;

        public WalletRepository(FinancialContext context) : base(context)
        {
            _financialContext = context;    
        }

        public async Task<Wallet> GetWalletByUserIdAsync(int userId)
        {
            return await _financialContext.Wallets.SingleOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
