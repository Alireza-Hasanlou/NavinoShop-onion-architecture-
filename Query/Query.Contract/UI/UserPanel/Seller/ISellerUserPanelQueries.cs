using Shared.Domain.Enums;
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
        Task<ProductsForSellerPaging> GetProductsForSellerAsync(int sellerId, int pageId, int take, string filter, int categoryId);
        Task<SellersOrdersPaging> GetSellersOrdersForUserPanelAsync(int SellerId, OrderSellerStatus status, int refId,int pageId, string filter = "");
        Task<OrderUserPanelViewModel> GetOrderDetailsForSellerAsync(int sellerId,int userId, int OrderId);


    }
}
