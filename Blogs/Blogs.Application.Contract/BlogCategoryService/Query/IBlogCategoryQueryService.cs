using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Application;

namespace Blogs.Application.Contract.BlogCategoryService.Query
{
    public partial interface IBlogCategoryQueryService
    {
        public interface IBlogCategoryQuery
        {
            Task<BlogCategoryAdminPageQueryModel> GetCategoriesForAdminAsync(int id);
            Task<List<BlogCategoryForCreateBlogQueryModel>> GetCategoriesForAddBlogAsync(int id);
            Task<EditBlogCategoryDto> GetForEditAsync(int id);
            Task<bool> CheckCategoryHaveParentAsync(int id);
        }

    }
}
