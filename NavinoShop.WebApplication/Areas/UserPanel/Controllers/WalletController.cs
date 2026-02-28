using Financial.Application.Contract.Transaction.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.UserPanel.Wallet;
using Shared.Application.Auth;
using Shared.Domain.Enums;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Route("/Profile/[action]")]
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class WalletController : Controller
    {
        private readonly IWalletQueryService _walletQueryService;
        private readonly IAuthService _authService;
        private readonly ITransactionQueries _transactionQueries;

        public WalletController(IWalletQueryService walletQueryService, IAuthService authService, ITransactionQueries transactionQueries)
        {
            _walletQueryService = walletQueryService;
            _authService = authService;
            _transactionQueries = transactionQueries;
        }

        public async Task<IActionResult> Wallet()
        {
            var userId = _authService.GetLoginUserId();
            var model = await _walletQueryService.GetWalletForUserPanel(userId);
            return View(model);
        }

        public async Task<JsonResult> LoadTransaction()
        {
            var userId = _authService.GetLoginUserId();
            var model = await _transactionQueries.GetTransactionsForUserAsync(userId, TransactionFor.Wallet);
            return Json(model);
        }
    }
}
