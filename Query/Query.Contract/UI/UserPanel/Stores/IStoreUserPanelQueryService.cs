using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.UserPanel.Stores
{
    public interface IStoreUserPanelQueryService
    {
        Task<List<StoresUserPanelQueryModel>> GetStores(int UserId);
        Task<StoreDetailsPaging> GetStoreDetails(int storeId);
        Task<List<ProductSellsForAddToStoreQueryModel>> GetProductSellsForAddToStore(int SellerId);
    }
}
