using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emails.Application.Contract.SensEmailService.Query
{
    public interface ISendEmailQueryService
    {
        Task<List<SendEmailQueryModel>> GetEmailSendsFoeAdmin();
        Task<SendEmailDetailQueryModel> GetSendEmailDetailForAdmin(int id);
    }
}
