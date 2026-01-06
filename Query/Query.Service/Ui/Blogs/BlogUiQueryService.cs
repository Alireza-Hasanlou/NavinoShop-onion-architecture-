using Blogs.Domain.BlogAgg;
using Blogs.Domain.BlogCategoryAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.UI;
using Query.Contract.UI.Blog;
using Seos.Domain.SeoAgg;
using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Service.Ui.Blogs
{
    internal class BlogUiQueryService : IBlogUiQueryService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly ISeoRepository _seoRepository;

        public BlogUiQueryService(IBlogRepository blogRepository, IBlogCategoryRepository blogCategoryRepository, ISeoRepository seoRepository)
        {
            _blogRepository = blogRepository;
            _blogCategoryRepository = blogCategoryRepository;
            _seoRepository = seoRepository;
        }

        public async Task<BlogsUiQueryPaging> GetBlogsForUi( string? categorySlug, int pageIndex,string? searchTerm)
        {
            var result = new BlogsUiQueryPaging();
            var categories = await _blogCategoryRepository
                .GetAllBy(c => c.Active)
                .AsNoTracking()
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Slug,
                    c.Parent
                })
                 .ToListAsync();

            IQueryable<Blog> blogsQuery = _blogRepository
                .GetAllBy(b => b.Active)
                .AsNoTracking();

            #region Category Filter 
            int seoOwnerId = 0;
            string seoTitle = "آرشیو مقالات";

            if (!string.IsNullOrWhiteSpace(categorySlug))
            {
                var category = categories.Single(s => s.Slug == categorySlug);

                blogsQuery = blogsQuery.Where(b =>
                    b.CategoryId == category.Id ||
                    b.SubCategoryId == category.Id);

                seoOwnerId = category.Id;
                seoTitle = category.Title;

                result.Title = categorySlug.Replace("_", " ");
                result.breadCrumbs = CreateBreadCrumb(categorySlug);
            }
            #endregion

            #region Search Filter 
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                result.Title = $"نتایج جستجو برای: {searchTerm}";

                blogsQuery = blogsQuery.Where(b =>
              b.Title.Contains(searchTerm)
           || b.Writer.Contains(searchTerm)
           || b.Text.Contains(searchTerm)
           || b.ShortDescription.Contains(searchTerm));
            }
            #endregion

            #region Paging
            result.GetData(blogsQuery, pageIndex, 2, 2);
            #endregion

            #region Blogs Query 
            var blogs = await blogsQuery
                .OrderByDescending(b => b.CreateDate)
                .Skip(result.Skip)
                .Take(result.Take)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Writer,
                    b.Slug,
                    b.ShortDescription,
                    b.CreateDate,
                    b.ImageAlt,
                    b.ImageName,
                    CategoryId = b.CategoryId > 0 ? b.CategoryId : b.SubCategoryId
                })
                .ToListAsync();
            #endregion

            #region Map Blogs
            result.Blogs = blogs.Select(b => new BlogsUiQueryModel
            {
                Id = b.Id,
                Title = b.Title,
                Writer = b.Writer,
                Slug = b.Slug,
                ShortDescription = b.ShortDescription,
                CreateDate = b.CreateDate.ToPersainDate(),
                ImageAlt = b.ImageAlt,
                ImageName = FileDirectories.BlogImageDirectory400 + b.ImageName,
                CategoryId = b.CategoryId,
                CategoryTitle = categories[b.CategoryId].Title,
                categorySlug = categories[b.CategoryId].Slug
            }).ToList();
            #endregion

            #region Categories

            var blogCounts = await _blogRepository
                .GetAllBy(b => b.Active)
                .GroupBy(b => b.CategoryId > 0 ? b.CategoryId : b.SubCategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.CategoryId, g => g.Count);

            result.blogCategories = categories
                .Where(c => c.Parent == 0)
                .Select(parent => new BlogCategoriesSerchQueryModel
                {
                    Id = parent.Id,
                    Title = parent.Title,
                    Slug = parent.Slug,
                    BlogCount = blogCounts.GetValueOrDefault(parent.Id),
                    Childs = categories
                        .Where(c => c.Parent == parent.Id)
                        .Select(child => new BlogCategoriesSerchQueryModel
                        {
                            Id = child.Id,
                            Title = child.Title,
                            Slug = child.Slug,
                            BlogCount = blogCounts.GetValueOrDefault(child.Id),
                            Childs = new()
                        }).ToList()
                }).ToList();
            #endregion

            #region SEO 
            var seo = await _seoRepository.GetSeoForUi(
                seoOwnerId,
                WhereSeo.BlogCategory,
                seoTitle);

            result.SeoUi = new SeoUiQueryModel
            {
                MetaTitle = seo.MetaTitle,
                MetaDescription = seo.MetaDescription,
                MetaKeyWords = seo.MetaKeyWords,
                Canonical = seo.Canonical,
                IndexPage = seo.IndexPage,
                Schema = seo.Schema
            };
            #endregion

            result.Filter = searchTerm;
            result.Slug = categorySlug;

            return result;
        }


        private List<BreadCrumb> CreateBreadCrumb(string slug)
        {
            return new List<BreadCrumb>()
            {
                new BreadCrumb
                {
                    Number=1,
                    Title="خانه",
                    Url="/Blog"

                },
                 new BreadCrumb
                {
                    Number=2,
                    Title="مقالات",
                    Url="/Blogs/"

                },
                  new BreadCrumb
                {
                    Number=3,
                    Title=slug.Trim().Replace("_"," "),
                    Url=""

                }
            };
        }
    }
}
