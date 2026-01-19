using Blogs.Domain.BlogAgg;
using Blogs.Domain.BlogCategoryAgg;
using Query.Contract.Admin.Seo;
using Shared.Domain.Enums;
using Site.Domain.SitePageAgg;

namespace Query.Service.Admin.Seo
{
    internal class SeoAdminQuery : ISeoAdminQuery
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly ISitePageRepository _sitePageRepository;

        public SeoAdminQuery(IBlogRepository blogRepository, IBlogCategoryRepository blogCategoryRepository, ISitePageRepository sitePageRepository)
        {
            _blogRepository = blogRepository;
            _blogCategoryRepository = blogCategoryRepository;
            _sitePageRepository = sitePageRepository;
        }

        public async Task<string> GetAdminSeoTitle(WhereSeo where, int ownerId)
        {
            switch (where)
            {
                case WhereSeo.Product:
                    return "";

                case WhereSeo.ProductCategory:
                    return "";

                case WhereSeo.Blog:
                    if (ownerId < 1) return "seo برای صفحه اصلی مقالات";
                    var blog = await _blogRepository.GetByIdAsync(ownerId);
                    if (blog == null) return "";
                    return $"Seo برای مقاله {blog.Title}";

                case WhereSeo.BlogCategory:
                    if (ownerId < 1) return "seo برای صفحه اصلی مقالات";
                    var blogCategory = await _blogCategoryRepository.GetByIdAsync(ownerId);
                    if (blogCategory == null) return "";
                    return $"Seo برای دسته بندی مقاله {blogCategory.Title}";

                case WhereSeo.Home:
                    return $"Seo برای صفخه اصلی ";

                case WhereSeo.About:
                    return $"Seo برای صفخه درباره ";

                case WhereSeo.Contact:
                    return $"Seo برای صفخه تماس با ما ";

                case WhereSeo.Page:
                    if (ownerId < 1) return "";
                    var page = await _sitePageRepository.GetByIdAsync(ownerId);
                    if (page == null) return "";
                    return $"Seo برای صفحه {page.Title}";

                case WhereSeo.PostPackage:
                    return $"Seo برای صفحه پست پکیج ";

                default:
                    return string.Empty;

            }
        }
    }
}
