namespace Site.Application.Contract.SiteSettingService.Query;

public class FooterUiQueryModel
{
    public FooterUiQueryModel(string? enamad, string? samanDehi, string? title,
        string? description, string? logoName, string? logoAlt,
        string? instagramLink, string? youtubeLink, string? telegramLink, string? whatsup,
        string? phone1, string? email1, string? android, string? ios)
    {
        Enamad = enamad;

        SamanDehi = samanDehi;
        Description = description;
        Title = title;
        LogoName = logoName;
        LogoAlt = logoAlt;
        InstagrmLink = instagramLink;
        YoutubeLink = youtubeLink;
        TelegramLink = telegramLink;
        WhatsupLink = whatsup;
        Phone1 = phone1;
        Email1 = email1;
        Android = android;
        Ios = ios;

    }

    public string? Enamad { get; private set; }
    public string? SamanDehi { get; private set; }
    public string? Title { get; private set; }
    public string? Description { get; private set; }
    public string? LogoName { get; private set; }
    public string? LogoAlt { get; private set; }
    public string? InstagrmLink { get; private set; }
    public string? YoutubeLink { get; private set; }
    public string? TelegramLink { get; private set; }
    public string? WhatsupLink { get; set; }
    public string? Phone1 { get; set; }
    public string? Email1 { get; set; }
    public string? Android { get; set; }
    public string? Ios { get; set; }


}