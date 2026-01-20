using Shared.Ui;

namespace Site.Application.Contract.SiteSettingService.Query;

public class ContactInfoForUiQueryModel
{


    public string Address { get; set; }
    public string Phone1 { get; set; }
    public string Phone2 { get; set; }
    public string Email { get; set; }
    public List<BreadCrumb> BreadCrumbs{ get; set; }

}