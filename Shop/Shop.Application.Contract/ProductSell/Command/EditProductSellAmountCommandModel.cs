using Shared.Domain.Enums;

namespace Shop.Application.Contract.ProductSell.Command
{


    public class EditProductSellAmountCommandModel
    {
        public int SellId { get; set; }
        public int count { get; set; }
        public StoreProductType Type { get; set; }
    }
}


