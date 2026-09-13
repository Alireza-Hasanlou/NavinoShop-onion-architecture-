using Shared.Application;
using Shop.Application.Contract.ProductView;
using Shop.Domain.ProductViewAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Commands
{
    internal class ProductViewCommands : IProductViewCommands
    {
        private readonly IProductViewRepository _productViewRepository;

        public ProductViewCommands(IProductViewRepository productViewRepository)
        {
            _productViewRepository = productViewRepository;
        }

        public async Task CreateAsync(CreateProductViewCommandModel model)
        {
            if (model.ProductId >= 0 && model.userId >= 0 &&  !string.IsNullOrEmpty(model.SessionId))
                await _productViewRepository.CreateAsync(new ProductView(model.userId, model.ProductId, model.SessionId));
        }

        public async Task<bool> IsExistProductViewAsync(int productId, string sessionId)=>
             await _productViewRepository.ExistByAsync( x => x.ProductId == productId && x.SessionId == sessionId);
        
    }
}
