
using Emails.Application.Contract.SendEmailService.Command;
using Emails.Domailn.SendEmailAgg;
using Shared.Application;
using Shared.Application.Validations;


namespace Emails.Application.Services
{
    internal class SendEmailService : ISendEmailCommandService
    {
        private readonly ISendEmailRepository _sendEmailRepository;
        public SendEmailService(ISendEmailRepository sendEmailRepository)
        {
            _sendEmailRepository = sendEmailRepository;
        }
        public async Task<OperationResult> CreateAsync(CreateSendEmailCommnadModel commmand)
        {
            SendEmail email = new(commmand.Title, commmand.Text);
            var result = await _sendEmailRepository.CreateAsync(email);
            if(result.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, nameof(commmand.Title));
        }
    }
}
