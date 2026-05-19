using Shared.Domain.Enums;

namespace Shop.Application.Contract.Seller.Query
{
    public class SellersChangeRequestQueryModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public string  WhyRejected { get; set; }
        public string SellerTitle { get; set; }
        public string ImageName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string Address { get; set; }
        public SellerStatus Status { get; set; }
    }
}
