using Shared.Application;
using Shared.Domain;
using Shop.Application.Contract.ProductSell.Command;

namespace Shop.Domain.ProductSellAgg
{
    public interface IProductSellRepository : IGenericRepository<ProductSell, int>
    {
        Task<OperationResult> EditProductSellAmountAsync(EditProductSellAmountCommandModel editAmountModel);
        Task<List<ProductSell>> GetByIdsAsync(List<int> productSellIds);
        Task<bool> ProductSellHaveAmount(int id , int quantity);
    }
}
