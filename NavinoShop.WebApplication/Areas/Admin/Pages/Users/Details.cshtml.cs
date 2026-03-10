using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.User;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly IAdminUserQueryService _adminUserQueryService;

        public DetailsModel(IAdminUserQueryService adminUserQueryService)
        {
            _adminUserQueryService = adminUserQueryService;
        }

        public UserDetailQueryModel UserDetailModel { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            if (Id < 1)
            {
                ViewData["success"] = "آیدی نا معتبر";
                return RedirectToPage("Index");
            }
            var userDetail = await _adminUserQueryService.GetUserDetailAsync(Id);
            if (userDetail != null)
            {
                UserDetailModel = userDetail;
                return Page();
            }
            ViewData["success"] = "خطا در بازگذاری اطلاعات";
            return Page();
        }
    }
}
