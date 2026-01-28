using Shared.Domain;
using Users.Application.Contract.UserAddressService.Query;

namespace Users.Domain.User.Agg.IRepository
{
    public interface IUserAddressRepository : IGenericRepository<UserAddress, int>
    {
        Task<UserAddressDto> GetAddressForEditAsync(int id);
    }
}
