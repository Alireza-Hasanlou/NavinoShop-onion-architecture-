using Shared.Application;
using Shared.Application.Validations;
using Shop.Application.Contract.Product_Seller.Command;
using Shop.Domain.Product_SellerAgg;

namespace Shop.Application.Commands
{
    internal class ProductSellerCommands : IProduct_Seller_Commands
    {
        private readonly IProduct_SellerRepository _product_SellerRepository;

        public ProductSellerCommands(IProduct_SellerRepository product_SellerRepository)
        {
            _product_SellerRepository = product_SellerRepository;
        }

        public Task<OperationResult> ActivationChangeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> CreateAsync(CreateProductSellerCommandModel command)
        {
            if (command.ProductId == 0)
                return new(false, ValidationMessages.RequiredMessage, nameof(command.ProductId));
            var ProductSeller = new Product_Seller(command.ProductId, command.Price, command.Unit, command.SellerId, command.Weight);
            var res = await _product_SellerRepository.CreateAsync(ProductSeller);
            if(res.Success)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Unit));

        }
        public async Task<OperationResult> EditAsync(EditProductSellerCommandModel command)
        {
            var p = await _product_SellerRepository.GetByIdAsync(command.Id);
            p.Edit(command.Price, command.Unit, command.Weight);
            if (await _product_SellerRepository.SaveAsync()) return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Unit));
 
        }


        public async Task<OperationResult> EditProductSellAmountAsync(List<EditProductSellerAmountCommandModel> sels)
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

        public async Task<EditProductSellerCommandModel> GetForEditAsync(int id)
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
