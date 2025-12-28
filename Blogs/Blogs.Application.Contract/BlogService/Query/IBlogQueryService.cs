using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Application.Contract.BlogService.Query
{
    public interface IBlogQueryService
    {
        Task<AdminBlogPageQueryModel> GetBlogsForAdmin(int CategoryId);
        Task<EditBlogQueryModel> GetForEditAsync(int id);
        Task<List<LastBlogsQueryModel>> GetLastBlogsAsync(int take);
    }
}
