using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emails.Application.Contract.SensEmailService.Command
{
    public interface ISendEmailCommandService
    {
        Task<OperationResult> Create(CreateSendEmailCommnadModel commmand);
    }
}
