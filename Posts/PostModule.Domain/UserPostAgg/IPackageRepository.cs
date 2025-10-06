using PostModule.Application.Contract.UserPostApplication.Command;
using Shared.Domain;

namespace PostModule.Domain.UserPostAgg
{
    public interface IPackageRepository : IGenericRepository<Package, int>
    {
        Task<CreatePostOrder> GetCreatePostModelAsync(int userId, int packageId);
        Task<EditPackage> GetForEdit(int id);
    }
}
