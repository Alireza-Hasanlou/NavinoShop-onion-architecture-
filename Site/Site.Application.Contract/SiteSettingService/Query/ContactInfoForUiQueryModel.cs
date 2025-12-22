namespace Site.Application.Contract.SiteSettingService.Query;

public class ContactInfoForUiQueryModel
{
    public ContactInfoForUiQueryModel(string address, string phone1,string phone2, string email)
    {
        Address = address;
        Phone1 = phone1;
        Phone2 = phone2;
        Email = email;
;
    }

    public string Address { get; set; }
    public string Phone1 { get; set; }
    public string Phone2 { get; set; }
    public string Email { get; set; }

}