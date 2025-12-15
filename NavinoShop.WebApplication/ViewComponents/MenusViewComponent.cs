using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.MenuService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class MenusViewComponent:ViewComponent
    {
        private readonly IMenuQueryService _menuQueryService;

        public MenusViewComponent(IMenuQueryService menuQueryService)
        {
            _menuQueryService = menuQueryService;
        }

        public async Task <IViewComponentResult> InvokeAsync()
        {
            var menus= await _menuQueryService.GetForIndexAsync();
            return View(menus);
        }
    }
}
