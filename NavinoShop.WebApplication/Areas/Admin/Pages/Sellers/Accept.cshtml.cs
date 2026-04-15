using Comments.Application.Contract.CommentService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.Seller.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Sellers
{
    public class AcceptModel : PageModel
    {
        private readonly ISellerCommands _sellerCommands;

        public AcceptModel(ISellerCommands sellerCommands)
        {
            _sellerCommands = sellerCommands;
        }

        public async Task<JsonResult> OnGet(int id)
        {

            if (id < 1)
                return new JsonResult(new { success = false });

            var result = await _sellerCommands.ChangeSellerStatus(id, Shared.Domain.Enums.SellerStatus.درخواست_تایید_شده,"");
            if (result.Success)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false , message= result.Message });
        }
    }
}

