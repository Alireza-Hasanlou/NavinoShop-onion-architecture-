using Shared.Application;
using Shared.Application.Validations;
using Shop.Application.Contract.ProductSell.Command;
using Shop.Domain.ProductSellAgg;

namespace Shop.Application.Commands
{
    internal class ProductSellCommands : IProductSellCommands
    {
        private readonly IProductSellRepository _productSellRepository;

        public ProductSellCommands(IProductSellRepository product_SellerRepository)
        {
            _productSellRepository = product_SellerRepository;
        }

        public async Task<OperationResult> ActivationChangeAsync(int sellerId, int id)
        {
            var productSell = await _productSellRepository.GetByIdAsync(id);
            if (productSell == null)
                return new OperationResult(false, "محصولی با شناسه ارسالی یافت نشد");

            productSell.ActivationChange();
            if (await _productSellRepository.SaveAsync())
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);

        }

        public async Task<OperationResult> CreateAsync(CreateProductSellCommandModel command)
        {
            if (command.ProductId == 0)
                return new(false, "لطفا محصول مورد نظر را انتخاب کنید", nameof(command.ProductId));
            if (await _productSellRepository.ExistByAsync(x => x.ProductId == command.ProductId))
                return new OperationResult(false, "محصول در حال حاضر در فروشگاه شما موجود است", nameof(command.ProductId));
            var ProductSell = new ProductSell(command.ProductId, command.Price, command.Unit, command.SellerId, command.Weight);
            var res = await _productSellRepository.CreateAsync(ProductSell);
            if (res.Success)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Unit));

        }

        public async Task<OperationResult> DeleteAsync(int Id)
        {
            var productSell = await _productSellRepository.GetByIdAsync(Id);
            if (productSell == null)
                return new(false, "محصولی با شناسه ارسالی یافت نشد");
            var res = await _productSellRepository.DeleteAsync(productSell);
            if (res.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> EditAsync(EditProductSellCommandModel command)
        {
            var p = await _productSellRepository.GetByIdAsync(command.Id);
            if (p == null)
                return new(false, "محصولی با شناسه ارسالی یافت نشد");
            p.Edit(command.Price, command.Unit, command.Weight);
            if (await _productSellRepository.SaveAsync()) return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Unit));

        }


        public async Task<OperationResult> EditProductSellAmountAsync(EditProductSellAmountCommandModel EditAmountModel)
        {

            var sell = await _productSellRepository.GetByIdAsync(EditAmountModel.SellId);
            if (sell == null)
                return new(false, "محصولی با شناسه ارسالی یافت نشد");
            sell.ChangeAmount(EditAmountModel.count, EditAmountModel.Type);

            if (await _productSellRepository.SaveAsync())
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<EditProductSellCommandModel> GetForEditAsync(int id)
        {
            var p = await _productSellRepository.GetByIdAsync(id);
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
