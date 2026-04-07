using Shared.Domain;
using Shop.Domain.ProductSellAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.SellerAgg
{
    public class Seller : BaseEntityCreateUpdateActive<int>
    {
        public Seller()
        {
            
        }
        public Seller(int userId, int title, int stateId, int cityId,
            string address, string mapUrl, string imageName, string licenseImage,
            string imageAlt, string? instagram, string? telegram, string? whatsup,
            string phone1, string? phone2, string? email)
        {
            UserId = userId;
            Title = title;
            StateId = stateId;
            CityId = cityId;
            Address = address;
            MapUrl = mapUrl;
            ImageName = imageName;
            LicenseImage = licenseImage;
            ImageAlt = imageAlt;
            Instagram = instagram;
            Telegram = telegram;
            Whatsup = whatsup;
            Phone1 = phone1;
            Phone2 = phone2;
            Email = email;
        }

        public int UserId { get; private set; }
        public int Title { get; private set; }
        public int StateId { get; private set; }
        public int CityId { get; private set; }
        public string Address { get; private set; }
        public string MapUrl { get; private set; }
        public string ImageName { get; private set; }
        public string LicenseImage { get; private set; }
        public string ImageAlt { get; private set; }
        public string? Instagram { get; private set; }
        public string? Telegram { get; private set; }
        public string? Whatsup { get; private set; }
        public string Phone1 { get; private set; }
        public string? Phone2 { get; private set; }
        public string? Email { get; private set; }
        public ICollection<ProductSell> ProductSells { get; private set; }
        public void Edit(int title, int stateId, int cityId,
    string address, string mapUrl, string imageName,
    string imageAlt, string? instagram, string? telegram, string? whatsup,
    string phone1, string? phone2, string? email)
        {
            Title = title;
            StateId = stateId;
            CityId = cityId;
            Address = address;
            MapUrl = mapUrl;
            ImageName = imageName;
            ImageAlt = imageAlt;
            Instagram = instagram;
            Telegram = telegram;
            Whatsup = whatsup;
            Phone1 = phone1;
            Phone2 = phone2;
            Email = email;
        }

        public void EditLicenseImage(string licenseImage)
        {
            LicenseImage = licenseImage;
        }
    }
}
