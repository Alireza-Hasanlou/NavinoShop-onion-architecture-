
using Emails.Application.Contract.MessageUserService.Command;
using Emails.Domailn.MessageUserAgg;
using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Application;

namespace Emails.Application.Services
{
    internal class MessageUserService : IMessageUserCommandService
    {
        private readonly IMessageUserRepository _messageUserRepository;
        public MessageUserService(IMessageUserRepository messageUserRepository)
        {
            _messageUserRepository = messageUserRepository;
        }

        public async Task<OperationResult> AnswerByCall(int id)
        {
            var messageUser = await _messageUserRepository.GetByIdAsync(id);
            messageUser.AnswerByCall();
            if (await _messageUserRepository.SaveAsync())
                return new(true);
            return new(false);
        }

        public async Task<OperationResult> AnsweredByEmail(int id, string mailMessage)
        {
			try
			{
				var messageUser = await _messageUserRepository.GetByIdAsync(id);
				messageUser.AnswerEmailSend(mailMessage);
				await _messageUserRepository.SaveAsync();
				//
				// send sms
				//
				return new(true);
			}
			catch (Exception)
			{
				return new(false);
			}
		}

        public async Task<OperationResult> AnsweredBySMS(int id, string message)
        {

            try
            {
				var messageUser = await _messageUserRepository.GetByIdAsync(id);
				messageUser.AnswerSmsSend(message);
				await _messageUserRepository.SaveAsync();
				//
				// send sms
				//
				return new(true);
			}
            catch (Exception)
            {
                return new(false);
            }
        }

        public async Task<OperationResult> Create(CreateMessageUserCommandModel command)
        {
            MessageUser messageUser = new(command.UserId, command.FullName, command.Subject, 
                command.PhoneNumber, command.Email, command.Message);
            var result = await _messageUserRepository.CreateAsync(messageUser);
            if(result.Success)
                return new(true);
            return new(false,ValidationMessages.SystemErrorMessage);  
        }
    }
}
