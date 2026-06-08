using Shared.Domain.Enums;

namespace Discount.Application.Contract.OrderDiscounts.Command
{
    public class UpsertOrderDiscountCommandModel
    {
        public int Id { get; set; }
        public int Percent { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public string Code { get; set; }
        public int ShopId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public OrderDiscountType type { get; set; }
    }
}
