using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Application.Contract.BlogCategoryService.Command
{
    public interface IBlogCategoryCommandService
    {
        Task<OperationResult> Create(CreateBlogCategoryCommand command);
        Task<OperationResult> Edit(EditBlogCategoryCommand command);
        Task<OperationResult> ActivationChange(int id);

    }
}
