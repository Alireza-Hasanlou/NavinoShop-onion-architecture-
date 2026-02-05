namespace WebApplication.Test.Controllers
{
    public partial class HomeController
    {
        public class PostPriceResponseApiModel
        {
          
            public List<PostPriceResponseModel> Prices { get;  set; }
            public string Message { get;  set; }
            public bool Success { get;  set; }
        }

    }
}
