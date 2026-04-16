using Shared.Domain.Enums;

namespace Query.Contract.Admin.Seller
{
    public class SellersRequrstAdminQueryModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string? WhyRejected { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string ImageName { get; set; }
        public string LicenseImage { get; set; }
        public string Phone1 { get; set; }
        public DateTime RequestDate { get; set; }
        public SellerStatus Status { get; set; }
    }
}
