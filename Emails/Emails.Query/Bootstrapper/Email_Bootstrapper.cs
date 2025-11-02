using Emails.Application.Contract.SendEmailService.Query;
using Emails.Infrastructure;
using Emails.Infrastructure.Bootstrapper;
using Emails.Query.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emails.Query.Bootstrapper
{
	public static class Email_Bootstrapper
	{
		public static void Config(IServiceCollection services,string connection)
		{
			EmailInfrastructureBootstrapper.Config(services, connection);
			services.AddTransient<ISendEmailQueryService,SendEmailQuery>();	
		}
	}
}
