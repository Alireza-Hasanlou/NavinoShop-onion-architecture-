using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostModule.Application.Contract.PostCalculate;
using PostModule.Application.Contract.StateQuery;

namespace NavinoShop.WebApplication.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly IPostCalculateApplication _postCalculate;
        private readonly IStateQueryService _stateQueryService;

        public PostController(IPostCalculateApplication postCalculate, IStateQueryService stateQueryService)
        {
            _postCalculate = postCalculate;
            _stateQueryService = stateQueryService;
        }

        [HttpPost]
        public async Task<IActionResult> CalculatePost(PostPriceRequestModel command)
        {
            var model = await _postCalculate.CalculatePost(command);
            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> CalculatePostForApi(PostPriceRequestApiModel command)
        {
            var model = await _postCalculate.CalculatePostForApi(command);
            return Ok(model);
        }
        [HttpGet]
        public async Task<List<StateQueryModel>> GetStatesWithCities()
        {
            return await _stateQueryService.GetStatesWithCity();
        }
        [HttpGet]
        public async Task<List<StateForChooseQueryModel>> States()
        {
            return await _stateQueryService.States();
            
        }
        [HttpGet]
        public async Task<List<CityForChooseQueryModel>> Cities(int id)
        {
            return await _stateQueryService.Cities(id);
        }
    }
}