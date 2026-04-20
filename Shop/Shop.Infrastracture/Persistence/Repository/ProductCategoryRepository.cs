using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Application.Contract.ProductCategory.Commands;
using Shop.Domain.ProductCategoryAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class ProductCategoryRepository : GenericRepository<ProductCategory, int>, IProductCategoryRepository
    {
        private readonly ShopContext _shopContext;
        public ProductCategoryRepository(ShopContext context) : base(context)
        {
            _shopContext = context;
        }

        public async Task<EditProductCategoryCommandModel> GetForEditAsync(int productCategoryId)
        {
            return await _shopContext.ProductCategories.Where(i => i.Id == productCategoryId)
                  .Select(x => new EditProductCategoryCommandModel
                  {
                      Id = productCategoryId,
                      Slug = x.Slug,
                      ParentId = x.ParentId,
                      Title = x.Title,
                  }).SingleAsync();
        }
    }
}
