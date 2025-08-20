namespace Users.Application.Contract.RoleService.Command
{
    public class EditRoleCommand : CreateRoleCommand
    {
        public int Id { get; set; }
    }
}
