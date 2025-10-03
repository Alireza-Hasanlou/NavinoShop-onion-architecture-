using Shared.Domain.Enums;

namespace Site.Application.Contract.MenuService.Query
{
    public class MenuForUiQueryModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public MenuStatus Status { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string? ImageName { get; set; }
        public string? ImageAlt { get; set; }
        public List<MenuForUiQueryModel>? Childs { get; set; }
    }
}
