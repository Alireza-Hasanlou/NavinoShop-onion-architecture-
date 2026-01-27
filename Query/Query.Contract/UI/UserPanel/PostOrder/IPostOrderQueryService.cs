using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.UserPanel.PostOrder
{
    public interface IPostOrderQueryService
    {
        Task<List<PostOrderUserPanelQueryModel>> GetPostOrderForUserPanelAsync(int userId);
    }
}