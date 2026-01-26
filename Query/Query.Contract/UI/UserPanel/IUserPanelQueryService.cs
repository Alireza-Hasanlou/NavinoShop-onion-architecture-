using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.UserPanel
{
    public interface IUserPanelQueryService
    {
        Task<UserPanelQueryModel> GetUserInfoForPanel(int id);
    }
}
