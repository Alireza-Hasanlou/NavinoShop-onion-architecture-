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
    }
}
