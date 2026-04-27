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
        public async Task<JsonResult> OnPost(int UserId, int Amount, string Description)
        {
            var AdminId = _authService.GetLoginUserId();

            var transationRes = await _transactionCommands.CreateAsync(new CreateTransacionCommandModel
            {
                UserId = UserId,
                Description = Description,
                Portal = TransactionPortal.دستی_ادمین,
                TransactionFor = TransactionFor.Wallet,
                TransactionSource = TransactionSource.توسط_ادمین,
                Price = Amount,
                TransactionType = TransactionType.واریز,
                Authority = "",
                TransationById = AdminId

            });
            if (transationRes.Success)
            {
              var transationId= Convert.ToInt64(transationRes.Data);
                var Depositres = await _walletCommands.DepositAsync(UserId, Amount, transationId);
                if (Depositres.Success)
                    return new JsonResult(new { success = true, message = "کیف پول کاربر با موفقیت شارژ شد" });

                return new JsonResult(new { success = false, message = Depositres.Message });
            }
            else
            {
                return new JsonResult(new { success = false, message = transationRes.Message });
            }

        }




    }
}
