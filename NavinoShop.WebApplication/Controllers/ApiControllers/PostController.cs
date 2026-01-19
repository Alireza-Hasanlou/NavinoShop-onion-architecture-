using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostModule.Application.Contract.PostCalculate;

namespace NavinoShop.WebApplication.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly IPostCalculateApplication _postCalculate;

        public PostController(IPostCalculateApplication postCalculate)
        {
            _postCalculate = postCalculate;
        }
        [HttpPost]
        public async Task<IActionResult> Get(PostPriceRequestModel command)
        {
            var model = await _postCalculate.CalculatePost(command);
            return Ok(model);
        }
    }
}
