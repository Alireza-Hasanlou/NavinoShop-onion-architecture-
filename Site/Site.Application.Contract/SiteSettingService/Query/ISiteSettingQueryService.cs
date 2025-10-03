using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Application.Contract.SiteSettingService.Query;

public interface ISiteSettingQueryService
{
    Task<SocialForUiQueryModel> GetSocialForUi();
    Task<LogoForUiQueryModel> GetLogoForUi();
    Task<FavIconForUiQueryModel> GetFavIconForUi();
    Task<FooterUiQueryModel> GetFooter();
    Task<ContactFooterUiQueryModel> GetContactDataForFooter();
}
