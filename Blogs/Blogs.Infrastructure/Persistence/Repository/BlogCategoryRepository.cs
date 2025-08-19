using Blogs.Application.Contract.BlogCategoryService.Query;
using Blogs.Domain.BlogCategoryAgg;
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
    internal class BlogCategoryRepository:GenericRepository<BlogCategory,int>,IBlogCategoryRepository
    {
        private readonly NavinoDbContext _context;

        public BlogCategoryRepository(NavinoDbContext context):base(context) 
        {
            _context = context;
        }

        public async Task<BlogCategory> GetBySlug(string slug)
        {
            return await _context.BlogCategories.SingleOrDefaultAsync(s => s.Slug == slug);
        }

        public Task<EditBlogCategoryDto> GetForEdit(int id)
        {
           return _context.BlogCategories.Where(i => i.Id == id).Select(i => new EditBlogCategoryDto
            {
                Id = i.Id,
                Slug = i.Slug,
                ImageAlt = i.ImageAlt,
                ImageFile = null,
                ImageName = i.ImageName,
                Parent = i.Parent,
                Title = i.Title
            }).SingleOrDefaultAsync();
        }
    }
}
