using Shared.Application;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Site.Application.Contract.MenuService.Command
{
    public class CreateSubMenuCommandModel : UbsertMenuCommandModel
    {

        public int ParentId { get; set; }
        public MenuStatus ParentStatus { get; set; }
        public string ParentTitle { get; set; }
    }
}
