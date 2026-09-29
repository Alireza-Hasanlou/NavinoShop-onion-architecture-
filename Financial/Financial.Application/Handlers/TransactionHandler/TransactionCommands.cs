
using Financial.Application.Contract.Transaction.Command;
using Financial.Application.Contract.WalletService.Commands;
using Financial.Domain.TransactionAgg;
using Financial.Domain.WalletAgg;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shared.Application.Validations;
using Shared.Domain.Enums;


namespace Financial.Application.Handlers.TransactionHandler
{
    internal class TransactionCommands : ITransactionCommands
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletCommands _walletCommands;

        public TransactionCommands(ITransactionRepository transactionRepository ,IWalletCommands walletCommands)
        {
            _transactionRepository = transactionRepository;
            _walletCommands=walletCommands;
        }

        public async Task<OperationResult> CreateAsync(CreateTransacionCommandModel commnad)
        {
            if (commnad.Price < 1000)
                return new OperationResult(false, ValidationMessages.PaymentPriceError, nameof(commnad.Price));
            if (!string.IsNullOrEmpty(commnad.Authority))
            {
                if (await _transactionRepository.ExistByAsync(a => a.Authority == commnad.Authority))
                    return new OperationResult(false, "عملیات ناموفق", nameof(commnad.Authority));
            }
            var newTransation = new Transaction(commnad.UserId, commnad.Price, commnad.Authority, commnad.Portal, TransactionStatus.نا_موفق
                , commnad.TransactionFor, commnad.TransactionType, commnad.TransactionSource, commnad.Description, commnad.TransationById);

            var res = await _transactionRepository.CreateAsync(newTransation);
            if (res.Success)
                return new OperationResult(true, "", "", newTransation.Id);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage, "", newTransation.Id);
        }

        public async Task<OperationResult> DeleteAsync(long transationId)
        {
            var transation = await _transactionRepository.GetByIdAsync(transationId);
            var res = await _transactionRepository.DeleteAsync(transation);
            return new OperationResult(res.Success);
        }

        public async Task<OperationResult> Payment(
       TransactionStatus status,
       long id,
       string refid)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
                return new OperationResult(
                    false,
                    ValidationMessages.SystemErrorMessage);

            const int maxRetries = 5;

            for (var attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    if (transaction.TransactionFor == TransactionFor.Wallet)
                        refid = GenerateRefId().ToString();

                    transaction.Payment(status, refid);

                    if (await _transactionRepository.SaveAsync())
                        return new OperationResult(true);

                    return new OperationResult(
                        false,
                        ValidationMessages.SystemErrorMessage);
                }
                catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
                {
                    if (transaction.TransactionFor != TransactionFor.Wallet)
                        return new OperationResult(
                            false,
                            ValidationMessages.SystemErrorMessage);

                    if (attempt == maxRetries - 1)
                        return new OperationResult(
                            false,
                            ValidationMessages.SystemErrorMessage);

                 
                }
            }

            return new OperationResult(
                false,
                ValidationMessages.SystemErrorMessage);
        }
        private static int GenerateRefId()
        {
            return Random.Shared.Next(100_000_000, 1_000_000_000);
        }
        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlException
                   && (sqlException.Number == 2601 || sqlException.Number == 2627);
        }

        public async Task<OperationResult> RefundToWalletAsync( int paymentPrice, int customerId ,string description,int transacitonby )
          
        {
            var transaction =
                await _transactionRepository.BeginTransactionAsync();

            try
            {
                var newTransaction = new CreateTransacionCommandModel
                {
                    Authority = "",
                    Description = description,
                    Portal = TransactionPortal.کیف_پول,
                    Price = paymentPrice,
                    TransactionFor = TransactionFor.Wallet,
                    TransactionSource = TransactionSource.بازگشت_ریز_فاکتور,
                    TransactionType = TransactionType.واریز,
                    TransationById = transacitonby,
                    UserId = customerId
                };

                var transactionResult =
                    await CreateAsync(newTransaction);

                if (!transactionResult.Success)
                {
                    await _transactionRepository
                        .RollbackTransactionAsync(transaction);

                    return new OperationResult(
                        false,
                        "خطا در ایجاد تراکنش بازگشت وجه");
                }

                long transactionId =
                    Convert.ToInt64(transactionResult.Data);

                var depositResult =
                    await _walletCommands.DepositAsync(
                        customerId,
                        paymentPrice,
                        transactionId);

                if (!depositResult.Success)
                {
                    await _transactionRepository
                        .RollbackTransactionAsync(transaction);

                    return new OperationResult(
                        false,
                        "خطا در بازگشت وجه به مشتری");
                }

                await _transactionRepository
                    .CommitTransactionAsync(transaction);

                return new OperationResult(
                    true,"","",
                    transactionId);
            }
            catch
            {
                await _transactionRepository
                    .RollbackTransactionAsync(transaction);

                return new OperationResult(
                    false,
                    "خطا در انجام عملیات بازگشت وجه");
            }
        }
    }
}
