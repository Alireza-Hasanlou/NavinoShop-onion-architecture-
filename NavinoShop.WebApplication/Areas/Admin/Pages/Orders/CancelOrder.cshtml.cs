using Financial.Application.Contract.Transaction.Command;
using Financial.Application.Contract.WalletService.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Application.Auth;
using Shared.Domain.Enums;
using Shop.Application.Contract.Order.Command;
using Shop.Application.Contract.Order.Query;
using Shop.Domain.OrderAgg;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Orders
{
    [IgnoreAntiforgeryToken]
    public class CancelOrderModel : PageModel
    {
        private readonly IOrderQueries _orderQueries;
        private readonly ITransactionCommands _transactionCommands;
        private readonly IOrderCommands _orderCommands;
        private readonly IWalletCommands _walletCommands;
        private readonly IAuthService _authService;
        private int _userId;

        public CancelOrderModel(IOrderQueries orderQueries, ITransactionCommands transactionCommands, 
            IOrderCommands orderCommands, IWalletCommands walletCommands , IAuthService authService)
        {
            _orderQueries = orderQueries;
            _transactionCommands = transactionCommands;
            _orderCommands = orderCommands;
            _walletCommands = walletCommands;
            _authService = authService;
        }

        public async Task<IActionResult> OnGet(int orderId)
        {
            if (orderId < 1)
                return NotFound();

            try
            {
                var order = await _orderQueries
                    .GetFactorforordercancellation(orderId, 0);

                if (order == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "سفارش یافت نشد"
                    });
                }
                _userId = _authService.GetLoginUserId();

                var refundResult = await _transactionCommands
                    .RefundToWalletAsync(
                        order.PaymentPrice,
                        order.CustomerId,
                        "واریز وجه به کیف پول مشتری به علت لغو سفارش توسط ادمین",
                        _userId);

                if (!refundResult.Success)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = refundResult.Message
                    });
                }

                long transactionId = Convert.ToInt64(refundResult.Data);


                var cancelResult = await _orderCommands
                    .CancellOrderByAdminAsync(orderId);


                if (cancelResult.Success)
                {
                    await _transactionCommands.Payment(
                        TransactionStatus.موفق,
                        transactionId,
                        string.Empty);

                    return new JsonResult(new
                    {
                        success = true,
                        message = "سفارش با موفقیت لغو و وجه آن به کیف پول شما بازگردانده شد"
                    });
                }


                var withdrawResult = await _walletCommands
                    .WithdrawAsync(
                        order.CustomerId,
                        order.PaymentPrice,
                        0);

                if (withdrawResult.Success)
                {
                    await _transactionCommands.Payment(
                        TransactionStatus.نا_موفق,
                        transactionId,
                        cancelResult.Message);

                    return new JsonResult(new
                    {
                        success = false,
                        message = "لغو سفارش انجام نشد و وجه به حالت قبل بازگردانده شد"
                    });
                }


                return new JsonResult(new
                {
                    success = false,
                    message = "لغو سفارش انجام نشد و خطایی در بازگردانی وضعیت مالی رخ داد"
                });
            }
            catch (Exception)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "خطایی در لغو سفارش رخ داد"
                });
            }
        }
    }
}
