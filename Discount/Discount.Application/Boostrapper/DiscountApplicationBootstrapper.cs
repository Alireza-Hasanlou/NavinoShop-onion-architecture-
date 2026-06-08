using Discount.Application.Commands;
using Discount.Application.Contract.OrderDiscounts.Command;
using Discount.Application.Contract.ProductDiscount.Command;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Boostrapper
{
    public static  class DiscountApplicationBootstrapper
    {
        public  static void Config(IServiceCollection services)
        {
            services. AddTransient<IProductDiscountCommands,ProductDiscountCommands>();
            services.AddTransient<IOrderDiscountsCommands, OrderDiscountsCommands>();
        }
    }
}
