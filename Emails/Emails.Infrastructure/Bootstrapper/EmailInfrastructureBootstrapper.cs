using Emails.Application;
using Emails.Application.Bootstrapper;
using Emails.Domailn.EmailAgg;
using Emails.Domailn.MessageUserAgg;
using Emails.Domailn.SendEmailAgg;
using Emails.Infrastructure.Persistence.Context;
using Emails.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Emails.Infrastructure.Bootstrapper
{
	public static class EmailInfrastructureBootstrapper
	{
		public static void Config(IServiceCollection services , string connection)
		{
			EmailApplicationBootstrapper.Config(services);
			services.AddTransient<IEmailUserRepository, EmailUserRepository>();
			services.AddTransient<ISendEmailRepository, SendEmailRepository>();
			services.AddTransient<IMessageUserRepository, MessageUserRepository>();

			services.AddDbContext<EmailContext>(x =>
			{
				x.UseSqlServer(connection);
			});
		}
	}
}
