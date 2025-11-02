
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Application.Contract.MenuService.Query
{
    public interface IMenuQueryService
    {
        Task<MenuPageAdminQueryModel>GetForAdminAsync(int parentId);
        Task<List<MenuForUiQueryModel>> GetForIndexAsync();
        Task<List<MenuForUiQueryModel>> GetForFooterAsync();
        Task<List<MenuForUiQueryModel>> GetForBlogAsync();

    }
}
