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
    }
}
