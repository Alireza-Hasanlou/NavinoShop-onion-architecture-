
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Application.Contract.MenuService.Query
{
    public interface IMenuQueryService
    {
        Task<MenuPageAdminQueryModel>GetForAdmin(int parentId);
        Task<List<MenuForUiQueryModel>> GetForIndex();
        Task<List<MenuForUiQueryModel>> GetForFooter();
        Task<List<MenuForUiQueryModel>> GetForBlog();

    }
}
