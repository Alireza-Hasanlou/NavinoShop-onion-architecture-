using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Application.Contract.BlogCategoryService.Query
{
    public partial interface IBlogCategoryQueryService
    {
        public interface IBlogCategoryQuery
        {
            BlogCategoryAdminPageQueryModel GetCategoriesForAdmin(int id);
            List<BlogCategoryForCreateBlogQueryModel> GetCategoriesForAddBlog(int id);
            bool CheckCategoryHaveParent(int id);
        }

    }
}
