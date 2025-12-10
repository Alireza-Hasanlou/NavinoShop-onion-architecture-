using Blogs.Application.Contract.BlogService.Query;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.BlogAgg
{
    public interface IBlogRepository:IGenericRepository<Blog,int>
    {
       Task< Blog> GetBySlug(string slug);
        Task< EditBlogQueryModel> GetForEdit(int id);
    }
}
