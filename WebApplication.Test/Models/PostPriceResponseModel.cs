namespace WebApplication.Test.Controllers
{
    public partial class HomeController
    {
        public class PostPriceResponseModel
        {
          
            public int PostId { get; set; }
            public string Title { get;  set; }
            public string Status { get;  set; }
            public int Price { get;  set; }
        }

    }
}
