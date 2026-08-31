using Shared.Application;
using Shared.Domain;
using Users.Application.Contract.UserAddressService.Query;

namespace Users.Domain.User.Agg.IRepository
{
    public interface IUserAddressRepository : IGenericRepository<UserAddress, int>
    {
        Task<UserAddressDto> GetAddressForEditAsync(int id);
        Task<UserAddress> GetDefaultAddressAsync(int userId);
        Task<int> GetUserAddressCount();
        Task<int> GetUserCityAsync(int userId);
        Task<OperationResult> SetAddressToDefaultAsync(int userId, int addressId);
    }
}
