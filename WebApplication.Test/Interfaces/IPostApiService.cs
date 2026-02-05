using Refit;
using WebApplication.Test.Controllers;
using static WebApplication.Test.Controllers.HomeController;

namespace WebApplication.Test.Interfaces
{
    public interface IPostApiService
    {
        [Get("/api/Post/States")]
        Task<List<State>> States();

        [Get("/api/Post/Cities/{id}")]
        Task<List<City>> Cities( int id);
        [Post("/api/Post/CalculatePostForApi")]
        Task<PostPriceResponseApiModel> Calculate([Body] PostPriceModel model);
    }

    public class City
    {
        public int CityCode { get; set; }
        public string Title { get; set; }
    }
    public class State
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
