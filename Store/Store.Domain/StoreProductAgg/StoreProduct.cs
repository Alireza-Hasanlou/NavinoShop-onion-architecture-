using Shared.Domain;
using Shared.Domain.Enums;

namespace Store.Domain.StoreProductAgg
{
    public class StoreProduct : BaseEntityCreate<int>
    {
        public StoreProduct()
        {
            Store = new();
        }

        public StoreProduct(int storeId, int prodcutSellId, StoreProductType storeProductType, int count)
        {
            StoreId = storeId;
            ProdcutSellId = prodcutSellId;
            StoreProductType = storeProductType;
            Count = count;
        }

        public int StoreId { get; private set; }
        public int ProdcutSellId { get; private set; }
        public StoreProductType StoreProductType { get; private set; }
        public int Count { get; private set; }
        public Store.Domain.StoreAgg.Store Store { get; private set; }

        
    }
}
