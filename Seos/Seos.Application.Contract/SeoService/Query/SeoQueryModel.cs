namespace Seos.Application.Contract.SeoService.Query;

public class SeoQueryModel
{
    public string MetaTitle { get; private set; }
    public string? MetaDescription { get; private set; }
    public string? MetaKeyWords { get; private set; }
    public bool IndexPage { get; private set; }
    public string? Canonical { get; private set; }
    public string? Schema { get; private set; }
    public SeoQueryModel(string metaTitle, string? metaDescription, string? metaKeyWords, bool indexPage, string? canonical, string? schema)
    {
        MetaTitle = metaTitle;
        MetaDescription = metaDescription;
        MetaKeyWords = metaKeyWords;
        IndexPage = indexPage;
        Canonical = canonical;
        Schema = schema;
    }


}