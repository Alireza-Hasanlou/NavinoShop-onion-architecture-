using Financial.Domain.TransactionAgg;
using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using PostModule.Domain.UserPostAgg;
using Query.Contract.UI.UserPanel;
using Query.Contract.UI.UserPanel.UserAddress;
using Shared.Application;
using Shared.Domain.Enums;
using Shop.Domain.SellerAgg;
using System.Net.Http.Headers;
using Users.Application.Contract.RoleService.Query;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Ui.UserPanel
{
    internal class UserPanelQueryService : IUserPanelQueryService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IPOstOrderRepository _postOrderRepository;
        private readonly ISellerRepository _sellerRepository;

        public UserPanelQueryService(IUserRepository userRepository, ITransactionRepository transactionRepository,
            IRoleQueryService roleQueryService, IPOstOrderRepository postOrderRepository, ISellerRepository sellerRepository)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _roleQueryService = roleQueryService;
            _postOrderRepository = postOrderRepository;
            _sellerRepository = sellerRepository;
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
                TransactionSum = transactions.Sum(p => p.Price),

            };

        }

        public async Task<UserPanelSieMneuQueryModel> GetUserSidePanelMenu(int userId)
        {
            // 1 Is Admin Panel
            var isAdmin = _roleQueryService.CheckPermission(userId, 1);
            var havePostOrder = _postOrderRepository.ExistByAsync(u => u.UserId == userId);
            var isSeller = _sellerRepository.ExistByAsync(u => u.UserId == userId);
            await Task.WhenAll(havePostOrder, isSeller);

            return new UserPanelSieMneuQueryModel
            {
                IsAdmim = isAdmin,
                IsSeller = isSeller.Result,
                HavePostOrder = havePostOrder.Result
            };
        }
    }
}
