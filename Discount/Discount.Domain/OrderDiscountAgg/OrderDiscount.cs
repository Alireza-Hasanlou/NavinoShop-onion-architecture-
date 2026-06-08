using Shared.Domain;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.OrderDiscountAgg
{
    public class OrderDiscount : BaseEntityCreate<int>
    {
        public OrderDiscount(int percent, string title, string code, int count,int shopId, DateTime startDate, DateTime endDate, OrderDiscountType orderDiscountType)
        {
            Percent = percent;
            Title = title;
            Code = code;
            Count = count;
            ShopId = shopId;
            StartDate = startDate;
            EndDate = endDate;
            OrderDiscountType = orderDiscountType;
            Use = 0;
        }

        public int Percent { get; private set; }
        public string Title { get; private set; }
        public string Code { get; private set; }
        public int Count { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int ShopId { get; private set; }
        public OrderDiscountType OrderDiscountType { get; private set; }
        public int Use { get; private set; }


        public void Edit(int percent, string title, string code, int count, DateTime startDate, DateTime endDate)
        {
            Percent = percent;
            Title = title;
            Code = code;
            Count = count;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void UsePlus()
        {
            Use++;
        }
        public void UseMinus()
        {
            Use--;
        }
    }
}