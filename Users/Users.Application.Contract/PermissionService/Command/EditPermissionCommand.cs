namespace Users.Application.Contract.PermissionService.Command
{
    public class EditPermissionCommand:CreatePermissionCommand
    {
        public int Id { get; set; }
    }
}