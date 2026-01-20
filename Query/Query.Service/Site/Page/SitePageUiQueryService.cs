using Blogs.Domain.BlogAgg;
using Query.Contract.Site.Page;
using Query.Contract.UI.Seo;
using Seos.Domain.SeoAgg;
using Shared.Domain.Enums;
using Shared.Ui;
using Site.Application.Contract.SitePageService.Query;
using Site.Domain.SitePageAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Service.Site.Page
{
    internal class SitePageUiQueryService : ISitePageUiQueryService
    {
        private readonly ISitePageRepository _sitePageRepository;
        private readonly ISeoRepository _seoRepository;

        public SitePageUiQueryService(ISitePageRepository sitePageRepository, ISeoRepository seoRepository)
        {
            _sitePageRepository = sitePageRepository;
            _seoRepository = seoRepository;
        }

        public async Task<PageUiQueryModel> GetPageAsync(string slug)
        {
            var site = await _sitePageRepository.GetBySlug(slug);
            #region SEO 
            var seo = await _seoRepository.GetSeoForUi(
                site.Id,
                WhereSeo.Page,
                site.Title);
            #endregion
            var model = new PageUiQueryModel()
            {
                Title = site.Title,
                Description = site.Description,
                Slug = site.Slug,
            };
            model.Seo = new SeoUiQueryModel
            {
                MetaTitle = seo.MetaTitle,
                MetaDescription = seo.MetaDescription,
                MetaKeyWords = seo.MetaKeyWords,
                Canonical = seo.Canonical,
                IndexPage = seo.IndexPage,
                Schema = seo.Schema
            };
            model.breadCrumbs = new List<BreadCrumb>
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
                    Number=4,
                    Title=site.Title,
                    Url=""

                }
            };

            return model;
        }
    }
}
