using Shared.Application;


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
