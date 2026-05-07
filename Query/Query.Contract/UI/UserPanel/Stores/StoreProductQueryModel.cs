using Shared.Domain.Enums;

namespace Query.Contract.UI.UserPanel.Stores
{
    public class StoreProductQueryModel
    {


        public int ProdcutSellId { get; set; }
        public string ProductSellTitle { get; set; }
        public DateTime CrateDate { get; set; }
        public StoreProductType StoreProductType { get; set; }
        public int Count { get; set; }
    }
}
