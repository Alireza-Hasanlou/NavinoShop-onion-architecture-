using Shared.Application;
using Shared.Application.Validations;
using Shop.Application.Contract.ProductFeature.Command;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductFreatureAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Commands
{
    internal class ProductFreatureCommands : IProductFeatureCommands
    {
        private readonly IProductFeatureRepository _productFeatureRepository;

        public ProductFreatureCommands(IProductFeatureRepository productFeatureRepository)
        {
            _productFeatureRepository = productFeatureRepository;
        }

        public async Task<OperationResult> CreateAsync(CreateProductFeatureCommandModel command)
        {
            if (await _productFeatureRepository.ExistByAsync(t => t.Title.Trim().ToLower() == command.Title.Trim().ToLower()))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

            if (command.ProductId < 1)
                return new OperationResult(false, "خطا در شناسه محصول!");
            var productFeature = new ProductFreature(command.ProductId, command.Title, command.Value);
            var res = await _productFeatureRepository.CreateAsync(productFeature);
            if (res.Success)
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);

        }

        public async Task<OperationResult> EditAsync(EditProductFeatureCommandModel command)
        {
            var productFeature = await _productFeatureRepository.GetByIdAsync(command.Id);
            if (productFeature == null)
                return new OperationResult(false, "خطا در پیدان کردن فیچر");
            if (await _productFeatureRepository.ExistByAsync(t => t.Title.Trim().ToLower() == command.Title.Trim().ToLower()))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

            productFeature.Edit(command.Title,command.Value);
            if(await _productFeatureRepository.SaveAsync())
                return new OperationResult(true);
            return new OperationResult (false, ValidationMessages.SystemErrorMessage);  

        }

        public async Task<EditProductFeatureCommandModel> GetForEditAsync(int FeatureId)
        {
           return await _productFeatureRepository.GetForEditAsync(FeatureId);
        }
    }
}
