using Shared.Domain.Enums;

namespace Query.Contract.Admin.Email.MessageUser
{
    public class MessageUserAdminQueryModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public MessageStatus Status { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string CreateDate { get; set; }
    }
}
