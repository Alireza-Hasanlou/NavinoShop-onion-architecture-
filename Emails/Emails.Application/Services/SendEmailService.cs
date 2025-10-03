
using Emails.Application.Contract.SensEmailService.Command;
using Emails.Domailn.SendEmailAgg;
using Shared.Application;
using Shared.Application.Validations;
using System.ComponentModel.Design;


namespace Emails.Application.Services
{
    internal class SendEmailService : ISendEmailCommandService
    {
        private readonly ISendEmailRepository _sendEmailRepository;
        public SendEmailService(ISendEmailRepository sendEmailRepository)
        {
            _sendEmailRepository = sendEmailRepository;
        }
        public async Task<OperationResult> Create(CreateSendEmailCommnadModel commmand)
        {
            SendEmail email = new(commmand.Title, commmand.Text);
            var result = await _sendEmailRepository.CreateAsync(email);
            if(result.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, nameof(commmand.Title));
        }
    }
}
