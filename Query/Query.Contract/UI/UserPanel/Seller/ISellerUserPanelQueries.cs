using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.UserPanel.Seller
{
    public interface ISellerUserPanelQueries
    {
        Task<List<SellerUserPanelQueryModel>> GetSellersForUserPanel(int UserId);

    }

}
