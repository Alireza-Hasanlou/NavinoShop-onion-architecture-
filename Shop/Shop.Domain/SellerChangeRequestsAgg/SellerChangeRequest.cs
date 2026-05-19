using Shared.Domain.Enums;
using Shop.Domain.SellerAgg;

namespace Shop.Domain.SellerChangeRequestsAgg
{
    public class SellerChangeRequest
    {
        public SellerChangeRequest(int sellerId, string avatar, string coverImage, string title,
            string address, string? googleMapUrl, string? whatsApp,
            string? telegram, string? instagram, string phone1,
            string? phone2, string? email, string description)
        {
            SellerId = sellerId;
            Avatar = avatar;
            CoverImage = coverImage;
            Title = title;
            Address = address;
            GoogleMapUrl = googleMapUrl;
            WhatsApp = whatsApp;
            Telegram = telegram;
            Instagram = instagram;
            Phone1 = phone1;
            Phone2 = phone2;
            Email = email;
            Description = description;
            status = SellerStatus.درخواست_ارسال_شده;
            RequestDate = DateTime.Now;
        }

        public int Id { get; private set; }
        public int SellerId { get; set; }
        public string Avatar { get; private set; }
        public string CoverImage { get; private set; }
        public string Title { get; private set; }
        public string Address { get; private set; }
        public string? GoogleMapUrl { get; private set; }
        public string? WhatsApp { get; private set; }
        public string? Telegram { get; private set; }
        public string? Instagram { get; private set; }
        public string Phone1 { get; private set; }
        public string? Phone2 { get; private set; }
        public string? Email { get; private set; }
        public string Description { get; private set; }
        public string? WhyRejected { get; set; }
        public DateTime RequestDate { get; private set; }
        public SellerStatus status { get; private set; }

        public Seller Seller { get; private set; }


        public void ChangeRequestStatus(SellerStatus _status, string whyRejected)
        {
            status = _status;
            if (!string.IsNullOrEmpty(whyRejected))
                WhyRejected = whyRejected;

        }


    }
}

