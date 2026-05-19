using Microsoft.AspNetCore.Http;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shop.Application.Contract.Seller.Query
{
    public class RequestDetailQueryModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public string AvatarImageName { get; set; }
        public string CoverImageName { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string? GoogleMapUrl { get; set; }
        public IFormFile? CoverImage { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? WhatsApp { get; set; }
        public string? Telegram { get; set; }
        public string? Instagram { get; set; }
        public string Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public SellerStatus Status { get; set; }

    }
}
