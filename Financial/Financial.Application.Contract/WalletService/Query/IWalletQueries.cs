using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Application.Contract.WalletService.Query
{
    public interface IWalletQueries
    {
        Task<bool> WalletHasAmountAsync(int userId, int Price);
    }

    
}
