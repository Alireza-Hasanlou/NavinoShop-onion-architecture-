using Shared.Domain.Enums;

namespace Query.Contract.Admin.Seller
{
    public class SellerRequestDetailQueryModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserImaneName { get; set; }
        public string Title { get;  set; }
        public string CityName { get; set; }
        public string Address { get;  set; }
        public string? MapUrl { get;  set; }
        public string ShopImageName { get;  set; }
        public string LicenseImage { get;  set; }
        public string ImageAlt { get;  set; }
        public string? Instagram { get;  set; }
        public string? Telegram { get;  set; }
        public string? Whatsup { get;  set; }
        public string Phone1 { get;  set; }
        public string? Phone2 { get;  set; }
        public string? Email { get;  set; }
        public string RequestDate { get; set; }
        public SellerStatus Status { get;  set; }
    }
}
