using Microsoft.AspNetCore.Mvc;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class SpecialsaleBanerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }

}