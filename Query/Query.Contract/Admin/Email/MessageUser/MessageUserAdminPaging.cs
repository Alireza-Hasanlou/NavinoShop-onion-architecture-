using Shared;
using Shared.Domain.Enums;

namespace Query.Contract.Admin.Email.MessageUser
{
    public class MessageUserAdminPaging:BasePaging
    {
        public List<MessageUserAdminQueryModel> Messages { get; set; }
        public string? Filter { get; set; }
        public MessageStatus Status { get; set; }
    }
}
