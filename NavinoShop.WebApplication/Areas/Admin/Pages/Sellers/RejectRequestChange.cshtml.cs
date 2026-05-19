using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.Seller.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Sellers
{
    [IgnoreAntiforgeryToken]
    
    public class RejectRequestChangeModel : PageModel
    {
        private readonly ISellerCommands _sellerCommands;

        public RejectRequestChangeModel(ISellerCommands sellerCommands)
        {
            _sellerCommands = sellerCommands;
        }
      
        public async Task<JsonResult> OnGet(int Id, string why)
        {
            var res = await _sellerCommands.RejectRequestChange(Id , why);
            if (res.Success)
                return new JsonResult(new { success = true, title = "درخواست ویرایش کاربر رد شد" });
            return new JsonResult(new { success = false, title = res.Message });
        }
    }
}
