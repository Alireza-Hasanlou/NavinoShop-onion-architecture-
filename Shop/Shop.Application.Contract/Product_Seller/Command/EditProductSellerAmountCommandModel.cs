using Shared.Domain.Enums;

namespace Shop.Application.Contract.Product_Seller.Command
{


    public class EditProductSellerAmountCommandModel
    {
        public int SellId { get; set; }
        public int count { get; set; }
        public StoreProductType Type { get; set; }
    }


