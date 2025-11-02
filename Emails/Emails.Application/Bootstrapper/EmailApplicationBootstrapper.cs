using Emails.Application.Contract.EmailUserService.Command;
using Emails.Application.Contract.MessageUserService.Command;
using Emails.Application.Contract.SendEmailService.Command;
using Emails.Application.Services;
using Microsoft.Extensions.DependencyInjection;
namespace Emails.Application.Bootstrapper
{
    public static class EmailApplicationBootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddTransient<IEmailUseCommandService, EmailUserService>();
            services.AddTransient<IMessageUserCommandService, MessageUserService>();
            services.AddTransient<ISendEmailCommandService, SendEmailService>();
        }
    }
}
