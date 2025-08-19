
using Blogs.Application.Contract.BlogCategoryService.Query;
using Utility.Shared.Domain;

namespace Blogs.Domain.BlogCategoryAgg
{
    public interface IBlogCategoryRepository:IGenericRepository<BlogCategory,int>
    {
       Task< BlogCategory> GetBySlug(string slug);
        Task <EditBlogCategoryDto> GetForEdit(int id);
    }
}
