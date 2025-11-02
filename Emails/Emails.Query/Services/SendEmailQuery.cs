
using Emails.Application.Contract.SendEmailService.Query;
using Emails.Domailn.SendEmailAgg;
using Microsoft.EntityFrameworkCore;
using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emails.Query.Services
{
	internal class SendEmailQuery : ISendEmailQueryService
	{
		private readonly ISendEmailRepository _sendEmailRepository;
        public SendEmailQuery(ISendEmailRepository sendEmailRepository)
        {
            _sendEmailRepository = sendEmailRepository;
        }
        public async Task<List<SendEmailQueryModel>> GetEmailSendsForAdmin()
		{
			return await _sendEmailRepository.GetAll()
				.Select(x => new SendEmailQueryModel()
				{
					CreationDate = x.CreateDate.ToPersainDate(),
					Id = x.Id,
					Title = x.Title
				}).ToListAsync();
		}

		public async Task<SendEmailDetailQueryModel> GetSendEmailDetailForAdmin(int id)
		{
			var email = await _sendEmailRepository.GetByIdAsync(id);
			return new()
			{
				CreationDate = email.CreateDate.ToPersainDate(),
				Id = email.Id,
				Text = email.Text,
				Title = email.Title
			};
		}
	}
}
