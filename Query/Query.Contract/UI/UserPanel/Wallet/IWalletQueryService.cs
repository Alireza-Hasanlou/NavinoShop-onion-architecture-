using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.UserPanel.Wallet
{
    public interface IWalletQueryService
    {
        Task<WalletDetailQueryModel> GetWalletForUserPanel(int userId);
    }
}
