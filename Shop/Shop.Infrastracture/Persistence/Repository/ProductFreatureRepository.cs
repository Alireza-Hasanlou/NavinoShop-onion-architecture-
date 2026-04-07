using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Application.Contract.ProductFeature.Command;
using Shop.Domain.ProductFreatureAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class ProductFreatureRepository : GenericRepository<ProductFreature, int>, IProductFeatureRepository
    {
        private readonly ShopContext _shopContext;

        public ProductFreatureRepository(ShopContext shopContext) : base(shopContext)
        {
            _shopContext = shopContext;
        }

        public async Task<EditProductFeatureCommandModel> GetForEditAsync(int featureId)
        {
            return await _shopContext.ProductFreatures.Where(i => i.Id == featureId)
                .Select(x => new EditProductFeatureCommandModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Value = x.Value,
                }).SingleAsync();
        }
    }
}
