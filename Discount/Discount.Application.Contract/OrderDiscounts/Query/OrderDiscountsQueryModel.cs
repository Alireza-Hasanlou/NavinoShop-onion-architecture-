namespace Discount.Application.Contract.OrderDiscounts.Query
{
    public class OrderDiscountsQueryModel
    {
        public int Id { get; set; }
        public int Percent { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public string Code { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Use { get; set; }
    }
}
