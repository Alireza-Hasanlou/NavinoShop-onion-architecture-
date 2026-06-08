namespace Discount.Application.Contract.ProductDiscount.Command
{
    public class UpsertProductDiscountCommandModel
    {
        public int ProductId { get; set; }
        public int ProductSellId { get; set; }
        public int DiscountPercent { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}
