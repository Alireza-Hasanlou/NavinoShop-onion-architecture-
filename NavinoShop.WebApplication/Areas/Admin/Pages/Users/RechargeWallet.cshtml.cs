using Financial.Application.Contract.Transaction.Command;
using Financial.Application.Contract.WalletService.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Application.Auth;
using Shared.Domain.Enums;


namespace NavinoShop.WebApplication.Areas.Admin.Pages.Users
{
    [IgnoreAntiforgeryToken]
    public class RechargeWalletModel : PageModel
    {
        private readonly IWalletCommands _walletCommands;
        private readonly ITransactionCommands _transactionCommands;
        private readonly IAuthService _authService;

        public RechargeWalletModel(IWalletCommands walletCommands, ITransactionCommands transactionCommands, IAuthService authService)
        {
            _walletCommands = walletCommands;
            _transactionCommands = transactionCommands;
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            return Partial("_RechargeWalletPartialView");
        }
        public async Task<JsonResult> OnPost(decimal Amount, int OwnerId, string Description)
        {
            var userId = _authService.GetLoginUserId();

            var Depositres = await _walletCommands.DepositAsync(userId, Amount, OwnerId,
             TransactionFor.Wallet,
             TransactionSource.توسط_ادمین,
             TransactionType.واریز,
             TransactionPortal.دستی_ادمین,
             Description, "");
            if (Depositres.Success)
            {

                return new JsonResult(new { success = true, message = "کیف پول کاربر با موفقیت شارژ شد" });

            }
            else
            {
                return new JsonResult(new { success = false, message = Depositres.Message });
            }

        }




    }
}
