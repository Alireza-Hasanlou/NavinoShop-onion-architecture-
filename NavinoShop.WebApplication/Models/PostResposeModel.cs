using PostModule.Application.Contract.PostCalculate;

namespace NavinoShop.WebApplication.Models
{
    public class PostResposeModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int sellerId { get; set; }
        public List<PostPriceResponseModel> Posts { get; set; }
    }
}
