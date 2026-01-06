namespace Query.Contract.UI
{
    public class SeoUiQueryModel
    {


        public string MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeyWords { get; set; }
        public bool IndexPage { get; set; }
        public string? Canonical { get; set; }
        public string? Schema { get; set; }
    }
}
