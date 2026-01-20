using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.Blogs
{
    public interface IBlogUiQueryService
    {
        Task<BlogsUiQueryPaging> GetBlogsForUi(string? slug, int pageId, string filter);
        Task<SingleBlogQueryModel> GetBlogForUi(string slug);
    }
}
