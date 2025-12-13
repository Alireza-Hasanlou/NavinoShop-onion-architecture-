using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Application.Contract.BlogService.Query
{
    public class AdminBlogPageQueryModel
    {
        public int CategoryId { get; set; }
        public string PageTitle { get; set; }
        public List<BlogQueryModel> Blogs { get; set; }
    }
}
