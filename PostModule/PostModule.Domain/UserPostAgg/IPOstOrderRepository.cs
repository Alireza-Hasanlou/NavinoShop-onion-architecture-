using PostModule.Application.Contract.UserPostApplication.Command;
using Shared.Domain;

namespace PostModule.Domain.UserPostAgg
{
    public interface IPOstOrderRepository : IGenericRepository<PostOrder, int>
    {
        Task<PostOrderUserPanelModel> GetPostOrderNotPaymentForUser(int userId);
        Task<PostOrder> GetPostOrderNotPaymentForUserAsync(int userId);
    }
}
