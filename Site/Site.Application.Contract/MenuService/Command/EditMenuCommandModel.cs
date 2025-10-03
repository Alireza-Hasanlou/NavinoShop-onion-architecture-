using Microsoft.AspNetCore.Http;
using Shared.Application;
using System.ComponentModel.DataAnnotations;

namespace Site.Application.Contract.MenuService.Command
{
    public class EditMenuCommandModel : UbsertMenuCommandModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string? ImageName { get; set; }
    }
}
