using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.MenuService.Query;

namespace NavinoShop.WebApplication.Areas.Blog.ViewComponents
{
    public class SideNavViewComponent:ViewComponent
    {
        private readonly IMenuQueryService _menuQueryService;

        public SideNavViewComponent(IMenuQueryService menuQueryService)
        {
            _menuQueryService = menuQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sidenav= await _menuQueryService.GetForBlogAsync();
            return View(sidenav);   
        }
    }
}
