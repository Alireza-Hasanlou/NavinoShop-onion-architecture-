using Shared.Domain.Enums;

namespace Site.Application.Contract.MenuService.Query
{
    public class MenuPageAdminQueryModel
    {
        public int ParentId { get; set; }
        public string PageTitle { get; set; }
        public MenuStatus? Status { get; set; }
        public List<MenuForAdminQueryModel> Menus { get; set; }
    }
}
