using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Application;

namespace Blogs.Application.Contract.BlogCategoryService.Command
{
    public interface IBlogCategoryCommandService
    {
        OperationResult Create(CreateBlogCategoryCommand command);
        OperationResult Edit(EditBlogCategoryCommand command);
        bool ActivationChange(int id);
        EditBlogCategoryCommand GetForEdit(int id);
    }
}
