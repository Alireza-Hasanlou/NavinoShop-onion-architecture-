
using Blogs.Application.Contract.BlogCategoryService.Query;
using Shared.Domain;

namespace Blogs.Domain.BlogCategoryAgg
{
    public interface IBlogCategoryRepository:IGenericRepository<BlogCategory,int>
    {
       Task< BlogCategory> GetBySlug(string slug);
        Task <EditBlogCategoryDto> GetForEdit(int id);
    }
}
