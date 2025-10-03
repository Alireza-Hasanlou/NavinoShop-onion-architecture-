using Shared.Application.BaseCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Application.Contract.BlogCategoryService.Command
{
    public class CreateBlogCategoryCommand:Title_Slug_Image_ImageAlt
    {
        public int Parent { get; set; }
    }
}
