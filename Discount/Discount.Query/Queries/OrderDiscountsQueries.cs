using Discount.Application.Contract.OrderDiscounts.Query;
using Discount.Domain.OrderDiscountAgg;
using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Query.Queries
{
    internal class OrderDiscountsQueries : IOrderDiscountsQueries
    {
        private readonly IOrderDiscountRepository _orderDiscountRepository;

        public OrderDiscountsQueries(IOrderDiscountRepository orderDiscountRepository)
        {
            _orderDiscountRepository = orderDiscountRepository;
        }

  

        public async Task<List<OrderDiscountsQueryModel>> GeAllAsync(int ShopId, OrderDiscountType type)
        {

            return await _orderDiscountRepository.GetAllBy(x => x.ShopId == ShopId && x.OrderDiscountType == type)
                .Select(x => new OrderDiscountsQueryModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Count = x.Count,
                    StartDate = x.StartDate.ToPersainDate(),
                    EndDate = x.EndDate.ToPersainDate(),
                    Percent = x.Percent,
                    Title = x.Title,
                    Use = x.Use,
                }).ToListAsync();

        }

        public async Task<List<OrderDiscountsQueryModel>> GeAllExpiredDiscountsAsync(int ShopId, OrderDiscountType type)
        {
            return await _orderDiscountRepository.GetAllBy(x => x.ShopId == ShopId && x.OrderDiscountType == type && x.EndDate < DateTime.Now)
                 .Select(x => new OrderDiscountsQueryModel
                 {
                     Id = x.Id,
                     Code = x.Code,
                     Count = x.Count,
                     StartDate = x.StartDate.ToPersainDate(),
                     EndDate = x.EndDate.ToPersainDate(),
                     Percent = x.Percent,
                     Title = x.Title,
                     Use = x.Use,
                 }).ToListAsync();
        }

        public async Task<OperationResultOrderDiscount> GetOrderSellerDiscountAsync(int sellerId, string code)
        {
            var discount = await _orderDiscountRepository.GetByCodeAsync(code);

            if (discount is null)
                return new(false, $"تخفیفی با کد {code} یافت نشد");
            if (sellerId == 0 && discount.ShopId != 0)
                return new(false, $"تخفیفی با کد {code} یافت نشد");
            if (sellerId > 0 && discount.ShopId != sellerId)
                return new(false, $"تخفیفی با کد {code} برای این فروشگاه ثبت نشده");
            if (discount.EndDate.Date < DateTime.Now.Date)
                return new(false, $"مهلت استفاده از کد تخفیف {code} به پایان رسیده ");
            if (discount.StartDate.Date > DateTime.Now.Date)
                return new(false, $"مهلت استفاده از کد تخفیف {code}هنوز شروع نشده تا تاریخ {discount.StartDate.Date.ToPersainDate()} صبرکنید");
            if (discount.Count < 1)
                return new(false, $"تخفیف با کد {code} به پایان رسیده");

            discount.UsePlus();
            if (!await _orderDiscountRepository.SaveAsync())
                return new(false, "خطا در بارگذاری تخفیف");
            return new(true, "", discount.Title, discount.Id, discount.Percent);

        }
    }
}
