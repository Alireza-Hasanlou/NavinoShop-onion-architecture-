using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Admin.Data
{
    public interface IAdminDataQueryService
    {
        Task<IEnumerable<MonthlySalesQueryModel>> GetMonthlySalesAsync();
        Task<SiteDataQueryModel> GetSiteDataAsync();
        Task<IEnumerable<WeeklySalesQueryModel>> GetWeeklySalesAsync();
        Task<NotificationQueryModel> GetNotificationForAdminAsync();
        
    }
    public class NotificationQueryModel
    {
        public int  NewMessagesCount { get; set; }
        public int NewRegisteredUsersCount { get; set; }
        public int NewOrderCount { get; set; }
        public int NewRequestForSellCount { get; set; }


    }
    public class MonthlySalesQueryModel
    {
        public int Month { get; set; }
        public int SalesCount { get; set; }
    }

    public class WeeklySalesQueryModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public int SalesCount { get; set; }
    }

    public class SiteDataQueryModel
    {
        public  int  SellersCount { get; set; }
        public int UsersCount { get; set; }
        public int RegisteredUsersOverThePastMonth { get; set; }
        public int BlogCount { get; set; }
        public int ProductCount { get; set; }
        public int Monthlysales { get; set; }
        public int Monthlyincome { get; set; }
        public int MonthlyProductVisitCount { get; set; }


    }
         
}
