using Microsoft.EntityFrameworkCore;
using PostModule.Application.Contract.PostSettingApplication.Command;
using PostModule.Domain.Services;
using PostModule.Domain.SettingAgg;
using PostModule.Domain.UserPostAgg;
using Query.Contract.UI;
using Query.Contract.UI.PostPackage;
using Query.Contract.UI.Seo;
using Seos.Domain.SeoAgg;
using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Service.Ui.PostPackages
{
    internal class PackagesUiQueryService : IPackageUiQueryService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPostSettingRepository _postSettingRepository;
        private readonly ISeoRepository _seoRepository;

        public PackagesUiQueryService(IPackageRepository packageRepository, IPostSettingRepository postSettingRepository, ISeoRepository seoRepository)
        {
            _packageRepository = packageRepository;
            _postSettingRepository = postSettingRepository;
            _seoRepository = seoRepository;
        }

        public async Task<PackagePageUiQueryModel> GetPackgesForUi()
        {

            var postSetting = await _postSettingRepository.GetSingle();
            var packages = await _packageRepository.GetAllBy()
                .AsNoTracking()
                .Select(p => new PackageUiQueryModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Count = p.Count,
                    ImageName = FileDirectories.PackageImageDirectory + p.ImageName,
                    ImageAlt = p.ImageAlt,
                    Price = p.Price.ToString()

                })
                .ToListAsync();
            #region BreadCrumbs
            var breadCrumbs = new List<BreadCrumb>()
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
                    Title="پکیج ها ",
                    Url="/Packages"

                }};
            #endregion
            #region SEO 
            var seo = await _seoRepository.GetSeoForUi(
                postSetting.Id,
                WhereSeo.PostPackage,
                postSetting.PackageTitle);

            var seoModel = new SeoUiQueryModel
            {
                MetaTitle = seo.MetaTitle,
                MetaDescription = seo.MetaDescription,
                MetaKeyWords = seo.MetaKeyWords,
                Canonical = seo.Canonical,
                IndexPage = seo.IndexPage,
                Schema = seo.Schema
            };
            #endregion
            PackagePageUiQueryModel model = new()
            {
                BreadCrumbs = breadCrumbs,
                Title = postSetting.PackageTitle,
                Description = postSetting.ApiDescription,
                packages = packages,
                Seo= seoModel
            };
            return model;
        }
    }
}
