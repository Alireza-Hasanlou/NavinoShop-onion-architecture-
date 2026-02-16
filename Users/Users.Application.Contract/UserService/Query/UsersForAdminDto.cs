namespace Users.Application.Contract.UserService.Query
{
    public class UsersForAdminDto
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
    }
}
