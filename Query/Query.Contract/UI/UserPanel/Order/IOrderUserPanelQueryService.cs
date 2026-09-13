using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.UserPanel.Order
{
    public interface IOrderUserPanelQueryService
    {
        Task<int> CalculateOrdersellerWeightAsync(int OrderSellerId);
        Task<OrderUserPanelViewModel> GetOrderAsync(int userId);
        Task<int> GetUserCityAsync(int userId);
        Task<List<OrdersForUserPanelQueryService>> GetOrdersAsync(int userId);
        Task<OrderUserPanelViewModel> GetOrderDetailsAsync(int userId, int orderId);
    }
}