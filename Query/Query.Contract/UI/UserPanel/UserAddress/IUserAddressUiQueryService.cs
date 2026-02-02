using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.UserPanel.UserAddress
{
    public interface IUserAddressUiQueryService
    {
        Task<List<UserAddressQueryModel>> GetUserAddressesAsync(int UserId);
    }
}
