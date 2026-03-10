using Financial.Domain.TransactionAgg;
using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.UI.UserPanel;
using Query.Contract.UI.UserPanel.UserAddress;
using Shared.Application;
using Shared.Domain.Enums;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Ui.UserPanel
{
    internal class UserPanelQueryService : IUserPanelQueryService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;

        public UserPanelQueryService(IUserRepository userRepository, ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<UserPanelQueryModel> GetUserInfoForPanel(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var transactions = await _transactionRepository.GetAllBy(u => u.OwnerId == id
           && u.TransactionFor == TransactionFor.Wallet
           && u.Status == TransactionStatus.موفق).ToListAsync();
            return new UserPanelQueryModel()
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                PhoneNumber = user.Mobile,
                RegisterDate = user.CreateDate.ToPersainDate(),
                Avatar = user.Avatar,
                Gender = user.UserGender,
                TransactionCount = transactions.Count(),
                TransactionSum = transactions.Sum(p=>p.Price),

            };

        }

       
    }
}
