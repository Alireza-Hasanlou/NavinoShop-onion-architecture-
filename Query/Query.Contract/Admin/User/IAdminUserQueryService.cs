using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Admin.User
{
    public interface IAdminUserQueryService
    {
        Task<UserDetailQueryModel> GetUserDetailAsync(int userId);
    }
}
