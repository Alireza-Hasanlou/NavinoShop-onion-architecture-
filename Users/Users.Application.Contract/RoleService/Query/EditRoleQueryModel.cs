using Users.Application.Contract.RoleService.Command;

namespace Users.Application.Contract.RoleService.Query
{
    public class EditRoleQueryModel:EditRoleCommand
    {
        public List<int >? permissions { get; set; }
    }
}