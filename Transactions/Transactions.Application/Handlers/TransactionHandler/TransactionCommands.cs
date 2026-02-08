using Domain.Entity;
using Shared.Application;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using Transactions.Application.Contract.Transaction.Command;

namespace Transactions.Application.Handlers.TransactionHandler
{
    internal class TransactionCommands : ITransactionCommands
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionCommands(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<OperationResult> CreateAsync(CreateTransacionCommandModel commnad)
        {
            if (commnad.Price < 1000)
                return new OperationResult(false, ValidationMessages.PaymentPriceError, nameof(commnad.Price));
            if (await _transactionRepository.ExistByAsync(a => a.Authority == commnad.Authority) || string.IsNullOrWhiteSpace(commnad.Authority))
                return new OperationResult(false, "عملیات ناموفق", nameof(commnad.Authority));
            var newTransation = new Transaction(commnad.UserId, commnad.Price, commnad.Authority, commnad.TransactionFor, commnad.OwnerId);

            var res = await _transactionRepository.CreateAsync(newTransation);
            if (res.Success)
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> Payment(TransactionStatus status, long id, string refid)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
                return new OperationResult(false, ValidationMessages.SystemErrorMessage);
            transaction.Payment(status, refid);
            if (await _transactionRepository.SaveAsync())
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }
    }
}
