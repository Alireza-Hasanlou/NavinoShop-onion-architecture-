using Shared;

namespace Query.Contract.UI.UserPanel.Stores
{
    public class StoreDetailsPaging : BasePaging
    {
        public int  SellerId { get; set; }
        public int StoreId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<StoreProductQueryModel> Details { get; set; }
    }
}
