using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Infrastructure.Bootstrapper;

namespace Users.Query.Bootstrapper
{
    public class UserQueryBootstrapper
    {
        public static void Config(IServiceCollection services, string connectionString)
        {
            UserInfrastructureBootstrapper.Config(services, connectionString);
        }
    }
}

