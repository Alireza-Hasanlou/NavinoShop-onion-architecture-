namespace PostModule.Application.Contract.PostPriceApplication
{
    public class EditPostPrice  : UbsertPostPrice
    {
        public int Id { get; set; }
        public int PostId { get; set; }
    }
}
