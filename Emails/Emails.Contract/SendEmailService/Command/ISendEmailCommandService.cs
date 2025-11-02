using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emails.Application.Contract.SendEmailService.Command
{
    public interface ISendEmailCommandService
    {
        Task<OperationResult> CreateAsync(CreateSendEmailCommnadModel commmand);
    }
}
