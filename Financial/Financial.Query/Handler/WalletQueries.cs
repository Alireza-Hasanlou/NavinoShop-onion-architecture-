using Financial.Application.Contract.WalletService.Query;
using Financial.Domain.WalletAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Query.Handler
{
    internal class WalletQueries : IWalletQueries
    {
        private readonly IWalletRepository _walletRepository;

        public WalletQueries(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public Task<bool> WalletHasAmountAsync(int userId, int Price)
        {
            return _walletRepository.ExistByAsync(x => x.OwnerId == userId && x.Balance >= Price);
        }
    }
}
