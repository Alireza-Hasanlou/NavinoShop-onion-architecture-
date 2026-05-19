using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Application.Contract.Product.Command;
using Shop.Application.Contract.Product.Query;
using Shop.Domain.ProductAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
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

        public async Task<Product> GetForAddRelToCategory(int id)
        {
            return await _shopContext.Products.Include(x => x.Poduct_Category_Rels).SingleAsync(i => i.Id == id);
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

            product.SelectedCategory = await _shopContext.product_Category_Rels.Where(i => i.ProductId == productId)
                .Select(c => c.CategoryId)
                 .Distinct()
                .ToListAsync();
            return product;

        }

        public async Task<List<ProductsForAddtoShopQueryModel>> GetProductsForAddToShop(List<int> categoryIds)
        {

            return await _shopContext.product_Category_Rels.Where(x => categoryIds.Contains(x.CategoryId))
                 .Include(x => x.Product)
                 .Select(x => new ProductsForAddtoShopQueryModel
                 {
                     Id = x.Product.Id,
                     Title = x.Product.Title

                 })
                 .Distinct()
                 .ToListAsync();
        }

        public Task<Product> GetWithCategoryRel(int id)
        {
            return _shopContext.Products.Where(i => i.Id == id)
                .Include(x => x.Poduct_Category_Rels)
                .SingleOrDefaultAsync();
        }
    }
}
