using Blogs.Application.Contract.BlogCategoryService.Query;
using Blogs.Application.Contract.BlogService.Query;
using Blogs.Domain.BlogAgg;
using Blogs.Domain.BlogCategoryAgg;
using Microsoft.EntityFrameworkCore;
using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
                model.PageTitle = $"مقالات مربوط به دسته {category.Title}";
                model.Blogs = await _blogRepository.GetAllBy(b => b.CategoryId == CategoryId || b.SubCategoryId == CategoryId).Select(a => new BlogQueryModel
                {
                    CategoryId = a.CategoryId,
                    Active = a.Active,
                    ImageName = a.ImageName,
                    Writer = a.Writer,
                    ShorDescription = a.ShortDescription,
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
                model.PageTitle = "همه مقالات";
                model.Blogs = await _blogRepository.GetAll().Select(a => new BlogQueryModel
                {
                    CategoryId = a.CategoryId,
                    Active = a.Active,
                    ImageName = a.ImageName,
                    Writer = a.Writer,
                    CreationDate = a.CreateDate.ToPersainDate(),
                    Title = a.Title,
                    ShorDescription = a.ShortDescription,
                    UpdateDate = a.UpdateDate.ToPersainDate(),
                    UserId = a.UserId,
                    VisitCount = a.VisitCount,
                    Id = a.Id,

                }).ToListAsync(); ;



            }
            return model;

        }

        public async Task<EditBlogQueryModel> GetForEditAsync(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);

            if (blog == null)
                return null;

            return new EditBlogQueryModel
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

        public async Task<List<LastBlogsQueryModel>> GetLastBlogsAsync(int take)
        {
            return await _blogRepository.GetAllBy(a => a.Active)
                .OrderByDescending(t => t.CreateDate)
                .Select(b => new LastBlogsQueryModel(b.Title, b.Slug))
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<MostViewedPosts>> GetMostViewedPostsAsync(int take)
        {
            var blogs = await _blogRepository.GetAllBy(a => a.Active)
                .OrderByDescending(s => s.VisitCount)
                .Select(b => new MostViewedPosts
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageName = b.ImageName,
                    ImageAlt = b.ImageAlt,
                    CreateDate = b.CreateDate.ToPersainDate(),
                    CategoryId = b.CategoryId,
                    SubCategoryId = b.SubCategoryId,
                    BlogSlug=b.Slug,
                    CategoryName = "",
                    CategorySlug = "",
                    Seen = b.VisitCount,
                    Writer = b.Writer
                }).Take(take)
                  .ToListAsync();

            foreach (var item in blogs)
            {
                if (item.SubCategoryId > 0)
                {
                    var C = await _categoryRepository.GetByIdAsync(item.SubCategoryId);
                    item.CategorySlug = C.Slug;
                    item.CategoryName = C.Title;
                }
                else
                {

                    var C = await _categoryRepository.GetByIdAsync(item.CategoryId);
                    item.CategorySlug = C.Slug;
                    item.CategoryName = C.Title;
                }
            }

            return blogs;
        }
    }
}
