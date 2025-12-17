using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.MenuService.Query;
namespace NavinoShop.WebApplication.ViewComponents
{
    public class FooterLinksViewComponent:ViewComponent
    {
        private readonly IMenuQueryService _menuQueryService;

        public FooterLinksViewComponent(IMenuQueryService menuQueryService)
        {
            _menuQueryService = menuQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Footer = await _menuQueryService.GetForFooterAsync();
            return View(Footer);
        }
    }
}
