using Blogs.Application.Contract.BlogApplication.Query;
using Blogs.Application.Contract.BlogCategoryService.Query;
using Blogs.Domain.BlogAgg;
using Blogs.Domain.BlogCategoryAgg;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Application;

namespace Blogs.Query.Services
{
    internal class BlogQuery : IBlogQueryService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogCategoryRepository _categoryRepository;

        public BlogQuery(IBlogRepository blogRepository, IBlogCategoryRepository categoryRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<AdminBlogPageQueryModel> GetBlogsForAdmin(int CategoryId)
        {

            AdminBlogPageQueryModel model = new AdminBlogPageQueryModel() { CategoryId = CategoryId };

            if (CategoryId > 0)
            {
                var category = await _categoryRepository.GetByIdAsync(CategoryId);
                model.PageTitle = $"مفالات مربوط به دسته {category.Title}";
                model.Blogs = await _blogRepository.GetAllBy(b => b.CategoryId == CategoryId || b.SubCategoryId == CategoryId).Select(a => new BlogQueryModel
                {
                    CategoryId = a.CategoryId,
                    Active = true,
                    ImageName = a.ImageName,
                    Writer = a.Writer,
                    CreationDate = a.CreateDate.ToPersainDate(),
                    Title = a.Title,
                    UpdateDate = a.UpdateDate.ToPersainDate(),
                    UserId = a.UserId,
                    VisitCount = a.VisitCount,
                    Id = a.Id,

                }).ToListAsync();

            }
            else
            {
                model.PageTitle = "همه مفالات";
                model.Blogs = await _blogRepository.GetAllAsync().Select(a => new BlogQueryModel
                {
                    CategoryId = a.CategoryId,
                    Active = true,
                    ImageName = a.ImageName,
                    Writer = a.Writer,
                    CreationDate = a.CreateDate.ToPersainDate(),
                    Title = a.Title,
                    UpdateDate = a.UpdateDate.ToPersainDate(),
                    UserId = a.UserId,
                    VisitCount = a.VisitCount,
                    Id = a.Id,

                }).ToListAsync(); ;



            }
            return model;

        }

        public async Task<EditBlogDto> GetForEditAsync(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);

            if (blog == null)
                return null;

            return new EditBlogDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Slug = blog.Slug,
                ImageAlt = blog.ImageAlt,
                ImageFile = null,
                ImageName = blog.ImageName,
                CategoryId = blog.CategoryId,
                ShortDescription = blog.ShortDescription,
                SubCategoryId = blog.SubCategoryId,
                Text = blog.Text,
                UserId = blog.UserId,
                Writer = blog.Writer

            };

        }
    }
}
