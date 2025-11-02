using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.ImageSiteService.Command;
using Users.Application.Contract.RoleService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Images
{
    [IgnoreAntiforgeryToken]
    public class DeleteModel : PageModel
    {
        private readonly IImageSiteCommandService _imageSiteService;

        public DeleteModel(IImageSiteCommandService imageSiteService)
        {
            _imageSiteService = imageSiteService;
        }


        
        public async Task<JsonResult> OnGet(int id)
        {
            var result = await _imageSiteService.DeleteFromDataBase(id);
            if (result.Success)
                return new JsonResult(new { success = true, title = "تصویر مورد نظر با موفقیت از دیتابیس حذف شد" });

            return new JsonResult(new { success = false, errors = result.Message });

        }
    }
}
