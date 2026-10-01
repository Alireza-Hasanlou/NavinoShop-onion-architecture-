using Query.Contract.Admin.Email.MessageUser;
using Query.Contract.Admin.Order;

namespace NavinoShop.WebApplication.Models
{
    public class AdminIndexPageViewModel
    {
        public List<LatestOrdersForIndexPage> LatestOrders { get; set; }
        public List<UnseenUsersMessageQueryModel> Messages { get; set; }
    }
}
