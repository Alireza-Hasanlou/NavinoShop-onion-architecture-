using PostModule.Application.Contract.UserPostApplication.Command;
using PostModule.Domain.SettingAgg;
using PostModule.Domain.UserPostAgg;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PostModule.Application.Services
{
    internal class UserPostApplication : IUserPostApplication
    {
        private readonly IPOstOrderRepository _postOrderRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IUserPostRepository _userPostRepository;
        private readonly IPostSettingRepository _postSettingRepository;
        public UserPostApplication(IPOstOrderRepository pOstOrderRepository, 
            IPackageRepository packageRepository, IUserPostRepository userPostRepository,
            IPostSettingRepository postSettingRepository)
        {
            _postOrderRepository = pOstOrderRepository;
            _packageRepository = packageRepository;
            _userPostRepository = userPostRepository;
            _postSettingRepository = postSettingRepository;
        }

        public async Task<bool> CreatePostOrderAsync(CreatePostOrder command)
        {
            var postOrder = await _postOrderRepository.GetPostOrderNotPaymentForUserAsync(command.UserId);
            if(postOrder == null)
            {
                postOrder = new(command.PackageId, command.UserId, command.Price);
                var result=  await   _postOrderRepository.CreateAsync(postOrder);
                if(result.Success)
                    return true;
                return false;
            }
            else
            {
                if(command.PackageId != postOrder.PackageId || command.Price != postOrder.Price)
                {
                    postOrder.Edit(command.PackageId, command.Price);
                    return await  _postOrderRepository.SaveAsync();
                }
                return true;
            }
        }

        public async Task<CreatePostOrder> GetCreatePostModelAsync(int userId, int packageId) =>
            await _packageRepository.GetCreatePostModelAsync(userId, packageId);

        public async Task<PostOrderUserPanelModel> GetPostOrderNotPaymentForUser(int userId) =>
            await _postOrderRepository.GetPostOrderNotPaymentForUser(userId);

        public async Task<UserPostPanelModel> GetUserPostModelForPanel(int userId)
        {
            UserPost userPost = await _userPostRepository.GetForUser(userId);
            var setting = await _postSettingRepository.GetSingle();
            return new UserPostPanelModel(setting.ApiDescription, userPost.Count, userPost.ApiCode);
        }

        public async Task<bool> PaymentPostOrderAsync(PaymentPostModel command)
        {
            var postOrder = await _postOrderRepository.GetPostOrderNotPaymentForUserAsync(command.UserId);
            if (postOrder == null || postOrder.Price != command.Price ||
                postOrder.Status == PostOrderStatus.پرداخت_شده ||
                postOrder.UserId != command.UserId) return false;
            postOrder.SuccessPayment(command.TransactionId);
            UserPost userPost = await _userPostRepository.GetForUser(command.UserId);
            var package = await _packageRepository.GetByIdAsync(postOrder.PackageId);
            userPost.CountPlus(package.Count);
            return await _userPostRepository.SaveAsync();
        }
    }
   
}
