using Shared.Domain;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.ProductDiscountAgg
{
    public class ProductDiscount : BaseEntityCreate<int>
    {


        public ProductDiscount(int productId, int productSellId, int percent, DateTime startDate, DateTime endDate)
        {
            ProductId = productId;
            ProductSellId = productSellId;
            Percent = percent;
            StartDate = startDate;
            EndDate = endDate;
            

        }

        public int ProductId { get; private set; }
        public int ProductSellId { get; private set; }
        public int Percent { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }


        public void Edit(int percent, DateTime startDate, DateTime endDate)
        {
            Percent = percent;
            StartDate = startDate;
            EndDate = endDate;
        }

    }
}
