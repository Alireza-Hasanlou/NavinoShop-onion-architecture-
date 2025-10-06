using Shared.Domain;

namespace PostModule.Domain.UserPostAgg
{
    public interface IUserPostRepository : IGenericRepository<UserPost, int>
    {
        Task<UserPost> GetForUser(int userId);
        Task<UserPost> GetByApiCode(string apiCode);
    }
}
