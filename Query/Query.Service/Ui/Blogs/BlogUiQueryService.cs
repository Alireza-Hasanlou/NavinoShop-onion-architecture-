using Blogs.Domain.BlogAgg;
using Blogs.Domain.BlogCategoryAgg;
using Comments.Domain.CommentAgg;
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
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Ui.Blogs
{
    internal class BlogUiQueryService : IBlogUiQueryService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly ISeoRepository _seoRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public BlogUiQueryService(IBlogRepository blogRepository,
            IBlogCategoryRepository blogCategoryRepository,
            ISeoRepository seoRepository,
            ICommentRepository commentRepository,
            IUserRepository userRepository)

        {
            _blogRepository = blogRepository;
            _blogCategoryRepository = blogCategoryRepository;
            _seoRepository = seoRepository;
            _commentRepository = commentRepository;
        }

        public async Task<SingleBlogQueryModel> GetBlogForUi(string slug)
        {

            var blog = await _blogRepository.GetBySlug(slug);
            if (blog == null)
                return null;
            int categoryId = blog.CategoryId > 0 ? blog.CategoryId : blog.SubCategoryId;
            var blogCategory = await _blogCategoryRepository.GetByIdAsync(categoryId);
            var result = new SingleBlogQueryModel();
            result.Id = blog.Id;
            result.Title = blog.Title;
            result.Writer = blog.Writer;
            result.Text = blog.Text;
            result.ImageName = FileDirectories.BlogImageDirectory + blog.ImageName;
            result.ImageAlt = blog.ImageAlt;
            result.CreateDate = blog.CreateDate.ToPersainDate();
            result.VisitCount = blog.VisitCount;
            result.CategoryTitle = blogCategory.Title;
            result.CategorySlug = blogCategory.Slug;
            result.CategoryId = categoryId;
            result.RelateBlogs = await _blogRepository.GetAllBy(c => c.CategoryId == categoryId || c.SubCategoryId == categoryId)
                .Select(b => new BlogsUiQueryModel
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
                    CategoryTitle = blogCategory.Title,
                    categorySlug = blogCategory.Slug,
                }).ToListAsync();

            result.BreadCrumbs = SingleBlogBreadCrumb(blog.Title, blogCategory.Title, blogCategory.Slug);

            #region SEO 
            var seo = await _seoRepository.GetSeoForUi(
                blog.Id,
                WhereSeo.Blog,
                blog.Title);

            result.Seo = new SeoUiQueryModel
            {
                MetaTitle = seo.MetaTitle,
                MetaDescription = seo.MetaDescription,
                MetaKeyWords = seo.MetaKeyWords,
                Canonical = seo.Canonical,
                IndexPage = seo.IndexPage,
                Schema = seo.Schema
            };
            #endregion

            result.CommentCount = _commentRepository.GetAllBy(c => c.OwnerId == blog.Id && c.CommentFor == CommentFor.مقاله).Count();
            return result;
        }

        public async Task<BlogsUiQueryPaging> GetBlogsForUi(string? categorySlug, int pageIndex, string? searchTerm)
        {
            var result = new BlogsUiQueryPaging();
            var categories = await _blogCategoryRepository
                .GetAllBy(c => c.Active)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Slug,
                    c.Parent
                })
                 .ToListAsync();

            IQueryable<Blog> blogsQuery = _blogRepository
                .GetAllBy(b => b.Active);
            int seoOwnerId = 0;
            string seoTitle = "آرشیو مقالات";
            result.Title = seoTitle;
            var category = categories.SingleOrDefault(s => s.Slug == categorySlug);

            #region Category Filter 

            if (!string.IsNullOrWhiteSpace(categorySlug))
            {
                blogsQuery = blogsQuery.Where(b =>
                    b.CategoryId == category.Id ||
                    b.SubCategoryId == category.Id);

                seoOwnerId = category.Id;
                seoTitle = category.Title;
                result.Title = category.Title;

            }
            #endregion

            #region BreadCrumb
            //use seo title for breadcrumb title
            result.breadCrumbs = BlogsBreadCrumb(seoTitle);

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
            result.GetData(blogsQuery, pageIndex, 4, 4);
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


        private List<BreadCrumb> BlogsBreadCrumb(string title)
        {
            return new List<BreadCrumb>()
            {
                new BreadCrumb
                {
                    Number=1,
                    Title="خانه",
                    Url="/"

                },
                 new BreadCrumb
                {
                    Number=2,
                    Title="مجله خبری ",
                    Url="/Blog"

                },
                 new BreadCrumb
                {
                    Number=3,
                    Title="آرشیو مقالات  ",
                    Url="/Blogs"

                },
                  new BreadCrumb
                {
                    Number=4,
                    Title=title,
                    Url=""

                }
            };
        }
        private List<BreadCrumb> SingleBlogBreadCrumb(string title, string categoryTitle, string categorySlug)
        {
            return new List<BreadCrumb>()
            {
               new BreadCrumb
                {
                    Number=1,
                    Title="خانه",
                    Url="/"

                },
                 new BreadCrumb
                {
                    Number=2,
                    Title="مجله خبری ",
                    Url="/Blog"

                },
                 new BreadCrumb
                {
                    Number=3,
                    Title="آرشیو مقالات ",
                    Url="/Blogs"

                },
                  new BreadCrumb
                {
                    Number=4,
                    Title=categoryTitle,
                    Url=$"/Blogs/{categorySlug}"

                },
                   new BreadCrumb
                {
                    Number=5,
                    Title=title,
                    Url=""

                }
            };
        }
    }
}
