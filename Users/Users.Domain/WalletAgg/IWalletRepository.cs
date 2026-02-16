using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.WalletAgg
{
    public interface IWalletRepository : IGenericRepository<Wallet, int>
    {

        Task<Wallet> GetWalletByUserIdAsync(int userId);
    }

}
