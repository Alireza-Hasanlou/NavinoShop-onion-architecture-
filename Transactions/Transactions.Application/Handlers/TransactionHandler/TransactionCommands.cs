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
                return new OperationResult(false, ValidationMessages.PaymentPriceError);
            if (await _transactionRepository.ExistByAsync(a => a.Authority == commnad.Authority) || string.IsNullOrWhiteSpace(commnad.Authority))
                return new OperationResult(false, "عملیات ناموفق", commnad.Authority);
            var newTransation = new Transaction(commnad.UserId, commnad.Price, commnad.Authority, commnad.TransactionFor, commnad.OwnerId);

            var res = await _transactionRepository.CreateAsync(newTransation);
            if (res.Success)
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public Task<OperationResult> Payment(TransactionStatus status, int id, string refid)
        {
            throw new NotImplementedException();
        }
    }
}
