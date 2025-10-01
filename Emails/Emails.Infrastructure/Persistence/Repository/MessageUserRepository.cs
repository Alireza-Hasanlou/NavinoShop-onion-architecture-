using Emails.Domailn.MessageUserAgg;
using Emails.Infrastructure.Persistence.Context;
using Utility.Shared.Insfrastructure;

namespace Emails.Infrastructure.Persistence.Repository;

internal class MessageUserRepository : GenericRepository<MessageUser, int>, IMessageUserRepository
{
	private readonly EmailContext _context;

	public MessageUserRepository(EmailContext context) : base(context)
	{
		_context = context;
	}
}
