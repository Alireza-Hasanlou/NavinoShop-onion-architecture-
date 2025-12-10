using Microsoft.Extensions.DependencyInjection;
using PostModule.Application.Contract.CityService;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostCalculate;
using PostModule.Application.Contract.PostPriceApplication;
using PostModule.Application.Contract.PostSettingApplication.Command;
using PostModule.Application.Contract.StateApplication;
using PostModule.Application.Contract.UserPostApplication.Command;
using PostModule.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Bootsrapper
{
    public class PostApplicationBootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddTransient<IStateApplication, StateApplication>();
            services.AddTransient<ICityApplication, CityApplication>();
            services.AddTransient<IPostApplication, PostApplication>();
            services.AddTransient<IPostPriceApplication, PostPriceApplication>();
            services.AddTransient<IPackageApplication, PackageApplication>();
            services.AddTransient<IUserPostApplication, UserPostApplication>();
            services.AddTransient<IPostCalculateApplication, PostCalculateApplication>();
            services.AddTransient<IPostSettingApplication, PostSettingApplication>();
        }
    }
}
