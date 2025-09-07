namespace Users.Application.Contract.RoleService.Query
{
    public class UsersRoleQuryModel
    {
        public string  RoleTitle { get; set; }
        public List<UsersQueryModel> Users { get; set; }
    }
}