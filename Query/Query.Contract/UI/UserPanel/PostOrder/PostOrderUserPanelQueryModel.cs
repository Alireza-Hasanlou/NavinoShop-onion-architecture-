using Shared.Domain.Enums;

namespace Query.Contract.UI.UserPanel.PostOrder
{
    public class PostOrderUserPanelQueryModel
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public string packageTitle { get; set; }
        public int PackagePrice { get; set; }
        public string Date { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; }
        public int TransactionId { get; set; }
        public string TransactionRef { get; set; }
        public PostOrderStatus postOrderStatus { get; set; }
    }
}
