
using Emails.Application.Contract.EmailUserService.Command;
using Emails.Domailn.EmailAgg;
using Shared.Application;
using Shared.Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emails.Application.Services
{
    internal class EmailUserService : IEmailUseCommandService

    {
        private readonly IEmailUserRepository _emailUserRepository;

        public EmailUserService(IEmailUserRepository emailUserRepository)
        {
            _emailUserRepository = emailUserRepository;
        }

        public async Task<OperationResult> ActivationChange(int id)
        {
            var email = await _emailUserRepository.GetByIdAsync(id);
            email.ActivationChange();
            if (await _emailUserRepository.SaveAsync())
                return new(true);
            return new(false);
        }

        public async Task<OperationResult> Create(CreateEmailUserCommandModel command)
        {
            if (await _emailUserRepository.ExistByAsync(e => e.Email.Trim().ToLower() == command.Email.Trim().ToLower()))
                return new(false, ValidationMessages.DuplicatedMessage);
            EmailUser emailUser = new(command.Email.Trim().ToLower(), command.UserId);
            var result = await _emailUserRepository.CreateAsync(emailUser);
            if (result.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }
    }
}
