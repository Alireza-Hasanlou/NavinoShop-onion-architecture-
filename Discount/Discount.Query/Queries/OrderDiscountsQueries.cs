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
    }
}
