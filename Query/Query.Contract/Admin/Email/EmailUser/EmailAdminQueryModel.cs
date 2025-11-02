namespace Query.Contract.Admin.Email.EmailUser
{
    public class EmailAdminQueryModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int  UserId { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
        public string CreationDate { get; set; }
    }
}
