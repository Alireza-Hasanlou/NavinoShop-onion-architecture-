using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.Seller.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Sellers
{
    public class AcceptRequestChangeModel : PageModel
    {
        private readonly ISellerCommands _sellerCommands;

        public AcceptRequestChangeModel(ISellerCommands sellerCommands)
        {
            _sellerCommands = sellerCommands;
        }

        public async Task<JsonResult> OnGet(int Id)
        {
            var res = await _sellerCommands.AcceptRequestChange(Id);
            if (res.Success)
                return new JsonResult(new { success = true, title = "درخواست ویرایش کاربر تایید شد" });
            return new JsonResult(new { success = false, title = res.Message });
        }
    }
}
