using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emails.Application.Contract.SendEmailService.Query
{
    public interface ISendEmailQueryService
    {
        Task<List<SendEmailQueryModel>> GetEmailSendsForAdmin();
        Task<SendEmailDetailQueryModel> GetSendEmailDetailForAdmin(int id);
    }
}
