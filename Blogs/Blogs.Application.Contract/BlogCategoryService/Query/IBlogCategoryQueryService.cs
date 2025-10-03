using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Application.Contract.BlogCategoryService.Query
{
    public partial interface IBlogCategoryQueryService
    {      
            Task<BlogCategoryAdminPageQueryModel> GetCategoriesForAdminAsync(int parentId);
            Task<List<BlogCategoryForCreateBlogQueryModel>> GetCategoriesForAddBlogAsync(int id);
            Task<EditBlogCategoryDto> GetForEditAsync(int id);
            Task<bool> CheckCategoryHaveParentAsync(int id);

    }
}
