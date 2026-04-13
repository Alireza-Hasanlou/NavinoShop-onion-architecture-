using Shared.Application;
using Shared.Application.Validations;
using Shop.Application.Contract.ProductSell.Command;
using Shop.Domain.ProductSellAgg;

namespace Shop.Application.Commands
{
    internal class ProductSellCommands : IProductSellCommands
    {
        private readonly IProductSellRepository _product_SellerRepository;

        public ProductSellCommands(IProductSellRepository product_SellerRepository)
        {
            _product_SellerRepository = product_SellerRepository;
        }

        public Task<OperationResult> ActivationChangeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> CreateAsync(CreateProductSellCommandModel command)
        {
            if (command.ProductId == 0)
                return new(false, ValidationMessages.RequiredMessage, nameof(command.ProductId));
            var ProductSell = new ProductSell(command.ProductId, command.Price, command.Unit, command.SellerId, command.Weight);
            var res = await _product_SellerRepository.CreateAsync(ProductSell);
            if(res.Success)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Unit));

        }
        public async Task<OperationResult> EditAsync(EditProductSellCommandModel command)
        {
            var p = await _product_SellerRepository.GetByIdAsync(command.Id);
            p.Edit(command.Price, command.Unit, command.Weight);
            if (await _product_SellerRepository.SaveAsync()) return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Unit));
 
        }


        public async Task<OperationResult> EditProductSellAmountAsync(List<EditProductSellAmountCommandModel> sels)
        {
            foreach (var item in sels)
            {
                var sell = await _product_SellerRepository.GetByIdAsync(item.SellId);
                sell.ChangeAmount(item.count, item.Type);
            }
            if( await _product_SellerRepository.SaveAsync())
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<EditProductSellCommandModel> GetForEditAsync(int id)
        {
            var p = await _product_SellerRepository.GetByIdAsync(id);
            return new()
            {
                Id = p.Id,
                Price = p.Price,
                SellerId = p.SellerId,
                Unit = p.Unit,
                Weight = p.Weight
            };
        }
    }
}
