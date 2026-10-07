using Blogs.Infrastructure.Persistence.Context;
using Emails.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Query.Contract.Admin.Data;
using Shared.Domain.Enums;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Infrastructure.Persistence.Context;

namespace Query.Service.Data
{
    internal class AdminDataQueryService : IAdminDataQueryService
    {
        private readonly ShopContext _shopContext;
        private readonly UserContext _userContext;
        private readonly BlogDbContext _blogContext;
        private readonly EmailContext _emailContext;

        public AdminDataQueryService(ShopContext shopContext, UserContext userContext,
            BlogDbContext blogContext, EmailContext emailContext)
        {
            _shopContext = shopContext;
            _userContext = userContext;
            _blogContext = blogContext;
            _emailContext = emailContext;
        }

        public async Task<IEnumerable<MonthlySalesQueryModel>> GetMonthlySalesAsync()
        {
            var now = DateAndTime.Now;

            var startDate = new DateTime(
                now.Year,
                now.Month,
                1
            ).AddMonths(-11);

            var sales = await _shopContext.OrderSellers
                .Where(x =>
                    (x.Status == OrderSellerStatus.ارسال_شده ||
                     x.Status == OrderSellerStatus.پرداخت_شده ||
                     x.Status == OrderSellerStatus.در_حال_آماده_سازی) &&
                    x.Order.CreateDate >= startDate)
                .SelectMany(x => x.OrderItems.Select(item => new
                {
                    x.Order.CreateDate,
                    item.Count
                }))
                .ToListAsync();

            var salesByMonth = sales
                .GroupBy(x => new
                {
                    x.CreateDate.Year,
                    x.CreateDate.Month
                })
                .ToDictionary(
                    x => (x.Key.Year, x.Key.Month),
                    x => x.Sum(item => item.Count)
                );

            return Enumerable
                .Range(0, 12)
                .Select(i =>
                {
                    var date = startDate.AddMonths(i);

                    return new MonthlySalesQueryModel
                    {
                        Month = date.Month,
                        SalesCount = salesByMonth.TryGetValue(
                            (date.Year, date.Month),
                            out var count)
                            ? count
                            : 0
                    };
                })
                .ToList();
        }

        public async Task<NotificationQueryModel> GetNotificationForAdminAsync()
        {
            var now = DateTime.Now.Date;
            var yesterday = now.AddDays(-1);

            return new NotificationQueryModel
            {
                NewMessagesCount = await _emailContext.MessageUsers
                .Where(x => x.Status == MessageStatus.دیده_نشده)
                .CountAsync(),
                NewOrderCount = await _shopContext.Orders
                .Where(x => x.OrderStatus == OrderStatus.پرداخت_نشده && x.CreateDate.Date <= now && x.CreateDate.Date>= now.AddDays(-5))
                .CountAsync(),
                NewRegisteredUsersCount = await _userContext.Users
                .Where(x => x.CreateDate.Date <= now && x.CreateDate.Date >= yesterday)
                .CountAsync(),
                NewRequestForSellCount = await _shopContext.Sellers
                .Where(x => x.Status == SellerStatus.درخواست_ارسال_شده)
                .CountAsync(),

            };
        }

        public async Task<SiteDataQueryModel> GetSiteDataAsync()
        {
            var oneMonthAgo = DateAndTime.Now.Date.AddMonths(-1);

            var userCount = await _userContext.Users
                .Where(x => !x.IsDelete)
                .CountAsync();

            var sellerCount = await _shopContext.Sellers
                .Where(x => x.Active)
                .CountAsync();

            var productCount = await _shopContext.productSells
                .Where(x => x.Active)
                .CountAsync();

            var blogCount = await _blogContext.Blogs
                .Where(x => x.Active)
                .CountAsync();

            var registeredUsersOverThePastMonth = await _userContext.Users
                .Where(x => x.Active && x.CreateDate >= oneMonthAgo)
                .CountAsync();

            var monthlySales = await _shopContext.OrderSellers
                .Where(x =>
                    x.Status == Shared.Domain.Enums.OrderSellerStatus.ارسال_شده &&
                    x.Order.CreateDate >= oneMonthAgo)
                .SelectMany(x => x.OrderItems)
                .SumAsync(x => x.Count);

            var orderSellers = await _shopContext.OrderSellers.Where(x => x.Status == OrderSellerStatus.ارسال_شده && x.Order.CreateDate >= oneMonthAgo)
                 .SelectMany(x => x.OrderItems)
                 .ToListAsync();

            var monthlyIncome = orderSellers.Sum(x => x.SumPriceAfterOff);

            var monthlyProductVisitCount = await _shopContext.ProductViews
                .Where(x => x.CreateDate >= oneMonthAgo)
                .CountAsync();

            return new SiteDataQueryModel
            {
                UsersCount = userCount,
                SellersCount = sellerCount,
                ProductCount = productCount,
                BlogCount = blogCount,
                RegisteredUsersOverThePastMonth = registeredUsersOverThePastMonth,
                Monthlysales = monthlySales,
                Monthlyincome = monthlyIncome,
                MonthlyProductVisitCount = monthlyProductVisitCount
            };
        }

        public async Task<IEnumerable<WeeklySalesQueryModel>> GetWeeklySalesAsync()
        {
            var today = DateAndTime.Now.Date;


            var daysSinceSaturday = ((int)today.DayOfWeek + 1) % 7;
            var startOfWeek = today.AddDays(-daysSinceSaturday);
            var endOfWeek = startOfWeek.AddDays(7);

            var sales = await _shopContext.OrderSellers
                .Where(x =>
                    (x.Status == OrderSellerStatus.ارسال_شده ||
                     x.Status == OrderSellerStatus.پرداخت_شده ||
                     x.Status == OrderSellerStatus.در_حال_آماده_سازی) &&
                    x.Order.CreateDate >= startOfWeek &&
                    x.Order.CreateDate < endOfWeek)
                .SelectMany(x => x.OrderItems.Select(item => new
                {
                    x.Order.CreateDate,
                    item.Count
                }))
                .ToListAsync();

            var result = Enumerable
                .Range(0, 7)
                .Select(day => new WeeklySalesQueryModel
                {
                    DayOfWeek = (DayOfWeek)((day + 6) % 7),

                    SalesCount = sales
                        .Where(x =>
                        {
                            var daysSinceSaturday =
                                ((int)x.CreateDate.DayOfWeek + 1) % 7;

                            return daysSinceSaturday == day;
                        })
                        .Sum(x => x.Count)
                })
                .ToList();

            return result;
        }
    }
}
