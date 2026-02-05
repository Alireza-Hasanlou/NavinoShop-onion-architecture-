namespace WebApplication.Test.Controllers
{
    public partial class HomeController
    {
        public class PostPriceModel
        {
            public string ApiCode { get; set; }
            public int SourceCityId { get; set; }
            public int DestinationCityId { get; set; }
            public int Weight { get; set; }
        }

    }
}
