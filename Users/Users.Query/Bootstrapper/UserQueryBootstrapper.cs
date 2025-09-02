using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.RoleService.Query;
using Users.Infrastructure.Bootstrapper;
using Users.Query.Service;

namespace Users.Query.Bootstrapper
{
    public class UserQueryBootstrapper
    {
        public static void Config(IServiceCollection services, string connectionString)
        {
            UserInfrastructureBootstrapper.Config(services, connectionString);
            services.AddScoped<IRoleQueryService,RoleQueryService>();   
        }
    }
}

