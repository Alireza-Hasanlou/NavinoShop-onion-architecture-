using Shared.Application;
using Shared.Application.Validations;
using Store.Application.Contract.StoreProduct.Command;
using Store.Domain.StoreProductAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Commands
{
    internal class StoreProductCommands : IStoreProductCommands
    {
        private readonly IStoreProductRepository _storeProductRepository;

        public StoreProductCommands(IStoreProductRepository storeProductRepository)
        {
            _storeProductRepository = storeProductRepository;
        }

        public async Task<OperationResult> DeleteAsync(int Id)
        {
            var storeProduct = await _storeProductRepository.GetByIdAsync(Id);
            if (storeProduct == null)
                return new(false, "محصولی با شناسه ارسالی یافت نشد");
            var res = await _storeProductRepository.DeleteAsync(storeProduct);
            if (res.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);

        }

        public async Task<OperationResult> CreateAsync(CreateStoreProductCommandModel command)
        {
                var produtcstore = new StoreProduct(command.StoreId, command.ProdcutSellId, command.StoreProductType, command.Count);
                var res = await _storeProductRepository.CreateAsync(produtcstore);
                if (res.Success)
                    return new(true);
                return new(false, ValidationMessages.SystemErrorMessage);
        }
    }
}
