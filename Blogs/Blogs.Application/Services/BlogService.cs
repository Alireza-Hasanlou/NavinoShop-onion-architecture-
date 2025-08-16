using Blogs.Application.Contract.BlogApplication.Command;
using Blogs.Domain.BlogAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Application;

namespace Blogs.Application.Services
{
    internal class BlogService : IBlogCommandService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public Task<OperationResult> ChangeActivationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> CreateAsync(CreateBlogCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> EditAsync(EditBlogCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> IncreaseVisitCountAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
