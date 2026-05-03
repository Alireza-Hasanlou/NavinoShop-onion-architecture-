using Shared.Application;

namespace Shop.Application.Contract.ProductSell.Command
{
    public partial interface IProductSellCommands
    {
        Task<OperationResult> CreateAsync(CreateProductSellCommandModel command);
        Task<OperationResult> EditAsync(EditProductSellCommandModel command);
        Task<EditProductSellCommandModel> GetForEditAsync(int id);
        Task<OperationResult> ActivationChangeAsync(int sellerId, int id);
        Task<OperationResult> EditProductSellAmountAsync(List<EditProductSellAmountCommandModel> sels);
    }
}   