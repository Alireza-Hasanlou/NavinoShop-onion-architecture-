using Comments.Application.Contract.CommentService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.Seller.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Sellers
{
    public class RejectModel : PageModel
    {
        private readonly ISellerCommands _sellerCommands;

        public RejectModel(ISellerCommands sellerCommands)
        {
            _sellerCommands = sellerCommands;
        }

        public async Task<JsonResult> OnGet(int Id, string why)
        {

            if (Id < 1 || string.IsNullOrEmpty(why))
                return new JsonResult(new { success = false });

            var result = await _sellerCommands.ChangeSellerStatus(Id, Shared.Domain.Enums.SellerStatus.درخواست_تایید_نشده, why);
            if (result.Success)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false });
        }
    }
}
