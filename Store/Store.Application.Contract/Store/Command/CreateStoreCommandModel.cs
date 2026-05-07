namespace Store.Application.Contract.Store.Command
{
    public class CreateStoreCommandModel
    {
        public int UserId { get; set; }
        public int  SellerId { get; set; }
        public string Description { get; set; }

    }
}
