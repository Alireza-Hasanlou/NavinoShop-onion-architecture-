using Query.Contract.UI.Seo;
using Shared.Ui;

namespace Query.Contract.UI.PostPackage
{
    public class PackagePageUiQueryModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<PackageUiQueryModel> packages { get; set; }
        public List<BreadCrumb> BreadCrumbs { get; set; }
        public SeoUiQueryModel Seo { get; set; }
    }
}
