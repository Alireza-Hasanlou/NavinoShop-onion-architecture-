using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Financial.Transaction;
using Shared.Domain.Enums;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Transactions
{
    public class IndexModel : PageModel
    {
        private readonly IAdminTransactionQueryService _adminTransactionQueryService;

        public IndexModel(IAdminTransactionQueryService adminTransactionQueryService)
        {
            _adminTransactionQueryService = adminTransactionQueryService;
        }

        [BindProperty(SupportsGet = true)]
        public TransactionsPagingModel TransactionsModel { get; set; }
        public async Task<IActionResult> OnGet(TransactionFor transactionFor, TransactionSource transactionSource,
            TransactionStatus transactionStatus, TransactionType transactionType, int userId, int take = 10, int pageId = 1, string filter = "")

        {

            TransactionsModel = await _adminTransactionQueryService.GetTransactionsForAdmin(take, pageId, filter,userId, transactionFor, transactionSource, transactionStatus, transactionType);
            return Page();
        }
    }
}
