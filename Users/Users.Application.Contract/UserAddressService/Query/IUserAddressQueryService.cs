using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contract.UserAddressService.Query
{
    public interface IUserAddressQueryService
    {
        Task<UserAddressDto> GetAddressForAddToFactorAsync(int id);
        Task<bool> IsAddressForUserAsync(int id, int userId);
    }
}
