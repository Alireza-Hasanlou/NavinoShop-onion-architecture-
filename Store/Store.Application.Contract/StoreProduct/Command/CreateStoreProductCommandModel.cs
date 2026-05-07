using Shared.Domain.Enums;

namespace Store.Application.Contract.StoreProduct.Command
{
    public class CreateStoreProductCommandModel
    {

        public int StoreId { get;  set; }
        public int ProdcutSellId { get;  set; }
        public StoreProductType StoreProductType { get;  set; }
        public int Count { get;  set; }
    }
}
