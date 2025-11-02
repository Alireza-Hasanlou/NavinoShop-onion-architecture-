namespace Query.Contract.Admin.Email.EmailUser
{
    public interface IEmailAdminQuery
    {
       Task< EmailUserAdminPaging> GetAllEmailForAdmin(int pageId, int take, string? filter = "");
    }
}
