using Emails.Domailn.SendEmailAgg;
using Emails.Infrastructure.Persistence.Context;
using Shared.Insfrastructure;

namespace Emails.Infrastructure.Persistence.Repository;

internal class SendEmailRepository : GenericRepository<SendEmail, int> , ISendEmailRepository
{
	private readonly EmailContext _context;

	public SendEmailRepository(EmailContext context) : base(context)
	{
		_context = context;
	}
}
