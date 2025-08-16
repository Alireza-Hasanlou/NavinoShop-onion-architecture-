using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Application;

namespace Blogs.Application.Contract.BlogApplication.Command
{
    public interface IBlogCommandService
    {
        Task<OperationResult> CreateAsync(CreateBlogCommand command);
        Task<OperationResult> EditAsync(EditBlogCommand command);
        Task<OperationResult> ChangeActivationAsync(int id);
        Task<OperationResult> IncreaseVisitCountAsync(int id);
      
    }
}
