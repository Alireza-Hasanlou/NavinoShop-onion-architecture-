namespace Shop.Application.Contract.ProductView
{
    public record CreateProductViewCommandModel(int userId, int ProductId, string SessionId); 
}
