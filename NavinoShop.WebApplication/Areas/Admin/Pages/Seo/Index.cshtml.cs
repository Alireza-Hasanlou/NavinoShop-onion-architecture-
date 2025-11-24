using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Seo;
using Seos.Application.Contract.SeoService.Command;
using Shared.Domain.Enums;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Seo
{
    public class IndexModel : PageModel
    {
        private readonly ISeoAdminQuery _seoAdminQuery;
        private readonly ISeoCommandService _seoCommandService;

        public IndexModel(ISeoAdminQuery seoAdminQuery, ISeoCommandService seoCommandService)
        {
            _seoAdminQuery = seoAdminQuery;
            _seoCommandService = seoCommandService;
        }
        [BindProperty]
        public CreateSeoCommandModel Seo { get; set; }
        public async Task<IActionResult> OnGet(int id, WhereSeo where)
        {
            ViewData["title"] = await _seoAdminQuery.GetAdminSeoTitle(where, id);
            Seo = _seoCommandService.GetSeoForEdit(id, where);
            return Page();

        }
        public async Task<IActionResult> OnPost(int id, WhereSeo where)
        {
            ViewData["title"] = await _seoAdminQuery.GetAdminSeoTitle(where, id);
            if (!ModelState.IsValid)
                return Page();

            var result = await _seoCommandService.UpsertSeo(Seo);
            if (result.Success)
            {
                TempData["success"]= "عملیات با موفقیت انجام شد";
                return RedirectToPage("index", new {id=Seo.OwnerId,where=Seo.Where});
            }
            ModelState.AddModelError($"Seo.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
