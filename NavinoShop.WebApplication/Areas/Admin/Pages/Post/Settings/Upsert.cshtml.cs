using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostSettingApplication.Command;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.Settings
{
    public class UpsertModel : PageModel
    {
        private readonly IPostSettingApplication _postSettingApplication;

        public UpsertModel(IPostSettingApplication postSettingApplication)
        {
            _postSettingApplication = postSettingApplication;
        }
        [BindProperty]
        public UpsertPostSetting UpsertPost { get; set; }
        public async Task<IActionResult> OnGet()
        {
            UpsertPost = await _postSettingApplication.GetForUpsert();
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
                return Page();  

            var res= await _postSettingApplication .Upsert(UpsertPost);
            if (res.Success)
            {
                ViewData["success"] = "عملیات با موفقیت انجام شد";
            }
            ModelState.AddModelError($"UpsertPost.{res.ModelName}", res.Message);
            return Page();
        }
    }
}
