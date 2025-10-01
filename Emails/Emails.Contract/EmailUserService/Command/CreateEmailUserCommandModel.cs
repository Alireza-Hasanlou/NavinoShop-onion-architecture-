namespace Emails.Application.Contract.EmailUserService.Command
{
    public class CreateEmailUserCommandModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
