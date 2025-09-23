using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Application.Contract.BlogCategoryService.Command
{
    public class EditBlogCategoryCommand:CreateBlogCategoryCommand
    {
        public int Id { get; set; }
        public string? ImageName { get; set; }
    }
}
