using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.PostPackage;

namespace NavinoShop.WebApplication.Controllers
{
    public class PostController : Controller
    {
        private readonly IPackageUiQueryService _packageUiQueryService;

        public PostController(IPackageUiQueryService packageUiQueryService)
        {
            _packageUiQueryService = packageUiQueryService;
        }

        [Route("/PostPackages")]
        public async Task<IActionResult> PostPackages()
        {
            var model = await _packageUiQueryService.GetPackgesForUi();
            return View(model);
        }

    }
}
