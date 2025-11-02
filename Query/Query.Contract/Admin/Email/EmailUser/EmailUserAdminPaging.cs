using Shared;


namespace Query.Contract.Admin.Email.EmailUser
{
    public class EmailUserAdminPaging:BasePaging
    {
        public List<EmailAdminQueryModel> Emails { get; set; }
        public string? Filter { get; set; }
    }
}
