using Query.Contract.UI.Seo;
using Shared.Ui;

namespace Query.Contract.Site.Page
{
    public class PageUiQueryModel
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public SeoUiQueryModel Seo { get; set; }
        public List<BreadCrumb> breadCrumbs { get; set; }

    }
}
