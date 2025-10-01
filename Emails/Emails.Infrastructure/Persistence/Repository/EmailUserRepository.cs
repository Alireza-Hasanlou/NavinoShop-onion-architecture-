using Emails.Domailn.EmailAgg;
using Emails.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Utility.Shared.Insfrastructure;

namespace Emails.Infrastructure.Persistence.Repository;

internal class EmailUserRepository : GenericRepository<EmailUser, int>, IEmailUserRepository
{
	private readonly EmailContext _context;

	public EmailUserRepository(EmailContext context) : base(context)
	{
		_context = context;
	}

	public async Task<bool> CreateList(List<EmailUser> emailUsers)
	{
		_context.EmailUsers.AddRange(emailUsers);
		return await SaveAsync();
	}

	public async Task< EmailUser> GetByEmail(string email)
	{
		return await _context.EmailUsers.SingleOrDefaultAsync(e => e.Email.Trim().ToLower() == email.ToLower().Trim());
	}
}
