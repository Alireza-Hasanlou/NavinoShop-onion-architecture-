using Shared.Application;
using Shared.Ui;
using Site.Application.Contract.SiteSettingService.Query;
using Site.Domain.MenuAgg;
using Site.Domain.SiteSettingAgg;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Query.Services;

internal class SiteSettingQuery : ISiteSettingQueryService
{
    private readonly ISiteSettingRepository _siteSettingRepository;

    public SiteSettingQuery(ISiteSettingRepository siteSettingRepository)
    {
        _siteSettingRepository = siteSettingRepository;
    }

    public async Task<AboutUsQueryModel> GetAboutUsForUi()
    {
        var model = await _siteSettingRepository.GetSingle();

        #region BreadCrumb
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
                    Title="درباره ما",
                    Url=""

                } };
        #endregion

        return new AboutUsQueryModel
        {
            Title = model.AboutTitle,
            text = model.AboutDescription,
            ImageName = FileDirectories.SiteImageDirectory + (string.IsNullOrWhiteSpace(model.AboutImageName) ? "" : model.AboutImageName)
        };
    }

    public async Task<ContactInfoForUiQueryModel> GetContactData()
    {
        var site = await _siteSettingRepository.GetSingle();
        #region BreadCrumb
        var breadCrumbs = new List<BreadCrumb>()
            {
               new BreadCrumb
                {
                    Number=1,
                    Title=" خانه / ",
                    Url="/"

                },
                 new BreadCrumb
                {
                    Number=2,
                    Title=" تماس با ما ",
                    Url=""

                } };
        #endregion
        return new ContactInfoForUiQueryModel()
        {
            Address = site.Address,
            Phone1 = site.Phone1,
            Phone2 = site.Phone2,
            Email = site.Email1,
            BreadCrumbs= breadCrumbs
        };
    }

    public async Task<FavIconForUiQueryModel> GetFavIconForUi()
    {
        var site = await _siteSettingRepository.GetSingle();
        return new FavIconForUiQueryModel(string.IsNullOrEmpty(site.FavIcon) ? "" : FileDirectories.SiteImageDirectory + site.FavIcon);
    }

    public async Task<FooterUiQueryModel> GetFooter()
    {
        var site = await _siteSettingRepository.GetSingle();
        return new FooterUiQueryModel(site.Enamad, site.SamanDehi, site.FooterTitle, site.FooterDescription,
            FileDirectories.SiteImageDirectory64 + site.LogoName, site.LogoAlt, site.Instagram, site.Youtube, site.Telegram, site.WhatsApp,
            site.Phone1, site.Email1, site.Android, site.IOS
            );
    }

    public async Task<LogoForUiQueryModel> GetLogoForUi()
    {
        var site = await _siteSettingRepository.GetSingle();
        return new LogoForUiQueryModel(string.IsNullOrEmpty(site.LogoName) ? "" : FileDirectories.SiteImageDirectory64 + site.LogoName, site.LogoAlt);
    }

    public async Task<SocialForUiQueryModel> GetSocialForUi()
    {
        var site = await _siteSettingRepository.GetSingle();
        return new SocialForUiQueryModel(site.Instagram, site.WhatsApp, site.Telegram, site.Youtube);
    }
}
