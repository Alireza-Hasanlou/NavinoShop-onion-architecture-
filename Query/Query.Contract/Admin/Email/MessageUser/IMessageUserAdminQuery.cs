using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Admin.Email.MessageUser
{
    public interface IMessageUserAdminQuery
    {
        Task<MessageUserAdminPaging> GetMessagesForAdmin(MessageStatus status,int pageId, int take, string Filter = "");
        Task<MessageUserDetailAdminQueryModel> GetMessageDetailForAdmin(int id);
        Task<List<UnseenUsersMessageQueryModel>> GetUnseenUsersMessageForIndexAsync();
    }

    public class UnseenUsersMessageQueryModel
    {
        public int  Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string  Subject { get; set; }
        public string Message { get; set; }
        public string ImageName { get; set; }
        public DateTime CreateTime { get; set; }
        public string createdAt { get; set; }
    }
}
