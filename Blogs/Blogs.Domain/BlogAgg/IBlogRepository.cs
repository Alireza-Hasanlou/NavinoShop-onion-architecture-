using Blogs.Application.Contract.BlogApplication.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Domain;

namespace Blogs.Domain.BlogAgg
{
    public interface IBlogRepository:IGenericRepository<Blog,int>
    {
       Task< Blog> GetBySlug(string slug);
        Task< EditBlogDto> GetForEdit(int id);
    }
}
