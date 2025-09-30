using Blogs.Application.Contract.BlogApplication.Query;
using Blogs.Domain.BlogAgg;
using Blogs.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Domain;
using Utility.Shared.Insfrastructure;

namespace Blogs.Infrastructure.Persistence.Repository
{
    internal class BlogRepository : GenericRepository<Blog, int>, IBlogRepository
    {
        private readonly NavinoDbContext _context;

        public BlogRepository(NavinoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Blog> GetBySlug(string slug)
        {
            return await _context.Blogs.SingleOrDefaultAsync(s => s.Slug == slug);
        }

        public async Task<EditBlogQueryModel> GetForEdit(int id)
        {
            return await _context.Blogs.Where(i => i.Id == id).Select(b => new EditBlogQueryModel
            {
                Id = b.Id,
                CategoryId = b.CategoryId,
                Slug = b.Slug,
                ImageFile = null,
                ImageAlt = b.ImageAlt,
                ImageName = b.ImageName,
                ShortDescription = b.ShortDescription,
                SubCategoryId = b.SubCategoryId,
                Text = b.Text,
                Title = b.Title,
                UserId = b.UserId,
                Writer = b.Writer
            }).SingleOrDefaultAsync();


        }
    }
}
