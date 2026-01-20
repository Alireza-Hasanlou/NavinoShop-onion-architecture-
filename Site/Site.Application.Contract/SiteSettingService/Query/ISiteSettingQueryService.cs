using Shared.Ui;
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
    Task<ContactInfoForUiQueryModel> GetContactData();
    Task<AboutUsQueryModel> GetAboutUsForUi();
}
public class AboutUsQueryModel
{
    public string Title { get; set; }
    public string text { get; set; }
    public string ImageName { get; set; }
    public List<BreadCrumb> BreadCrumbs{ get; set; }
}
     