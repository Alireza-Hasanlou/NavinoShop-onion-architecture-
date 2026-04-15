using Shared.Domain.Enums;

namespace Query.Contract.UI.UserPanel.Seller
{
    public class SellerUserPanelQueryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string ImageName { get; set; }
        public string Phone { get; set; }
        public SellerStatus SellerStatus { get; set; }
        public string CreateDate { get; set; }
        public string whyRejected { get; set; }
    }
}
