using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Application.Contract.Product.Command;
using Shop.Domain.ProductAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        private readonly ShopContext _shopContext;
        public ProductRepository(ShopContext context) : base(context)
        {
            _shopContext = context;
        }

        public async Task<EditProductCommandModel> GetForEditAsync(int productId)
        {
            var product = await _shopContext.Products.Where(i => i.Id == productId)
                   .Select(i => new EditProductCommandModel
                   {
                       Id = productId,
                       Title = i.Title,
                       ImageName = i.ImageName,
                       ShortDescription = i.ShortDescription,
                       ImageAlt = i.ImageAlt,
                       Text = i.Description,
                       Weight = i.Weight,
                       Slug = i.Slug,
                   }).SingleAsync();

            product.CategoryIds = await _shopContext.product_Category_Rels.Where(i => i.ProductId == productId)
                .Select(c => c.CategoryId)
                .ToListAsync();
            return product;

        }
    }
}
