using Microsoft.VisualBasic;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Admin.Seller
{
    public interface IAdminSellerQueryService
    {
        Task<List<SellersRequrstAdminQueryModel>> GetAllSalesRequrstForAdmin();
        Task<SellerRequestDetailQueryModel> GetSellerRequestDetail(int Id);

    }
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
