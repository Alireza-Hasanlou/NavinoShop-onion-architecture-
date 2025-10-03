using Shared.Application;
using Site.Application.Contract.SiteSettingService.Query;
using Site.Domain.MenuAgg;
using Site.Domain.SiteSettingAgg;
using System;
using System.Collections.Generic;
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

    public async Task<ContactFooterUiQueryModel> GetContactDataForFooter()
    {
        var site = await _siteSettingRepository.GetSingle();
        return new ContactFooterUiQueryModel(site.Address, site.Phone1, site.Email1, site.Android, site.IOS);
    }

    public async Task< FavIconForUiQueryModel> GetFavIconForUi()
    {
        var site =await _siteSettingRepository.GetSingle();
        return new FavIconForUiQueryModel(string.IsNullOrEmpty(site.FavIcon) ? "" : FileDirectories.SiteImageDirectory64 + site.FavIcon);
    }

    public async Task< FooterUiQueryModel> GetFooter()
    {
        var site = await _siteSettingRepository.GetSingle();
        return new FooterUiQueryModel(site.Enamad, site.SamanDehi, site.FooterTitle, site.FooterDescription);
    }

    public async Task< LogoForUiQueryModel> GetLogoForUi()
    {
        var site = await _siteSettingRepository.GetSingle();
        return new LogoForUiQueryModel(string.IsNullOrEmpty(site.LogoName) ? "" : FileDirectories.SiteImageDirectory300 + site.LogoName, site.LogoAlt);
    }

    public async Task<SocialForUiQueryModel> GetSocialForUi()
    {
        var site = await _siteSettingRepository.GetSingle();
        return new SocialForUiQueryModel(site.Instagram, site.WhatsApp, site.Telegram, site.Youtube);
    }
}
