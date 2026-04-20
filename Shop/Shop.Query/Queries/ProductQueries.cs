using Microsoft.EntityFrameworkCore;
using Shop.Application.Contract.Product.Query;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.Relations.ProductCategoryRel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Query.Queries
{
    internal class ProductQueries : IProductQueries
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProduct_Category_Repository _product_Category_Repository;

        public ProductQueries(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, IProduct_Category_Repository product_Category_Repository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _product_Category_Repository = product_Category_Repository;
        }

        public async Task<ProductsForAdminPaging> GetAllProductsForAdmin(int pageId, int take, string filter, int categoryId)
        {
            var model = new ProductsForAdminPaging();
            var products = _productRepository.GetAll();

            if (categoryId > 0)
            {
                var category = await _productCategoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                    return new ProductsForAdminPaging();

                var productIds = _product_Category_Repository.GetAllBy(x => x.CategoryId == categoryId)
                    .Select(x => x.ProductId);

                products = products.Where(p => productIds.Contains(p.Id));
                model.Title = $"محصولات در دسته بندی {category.Title}";
                model.CategoryId = categoryId;
            }

          
            if (!string.IsNullOrEmpty(filter))
            {
                products = products.Where(p => p.Title.Contains(filter)
                                            || p.Description.Contains(filter)
                                            || p.Slug.Contains(filter));
            }

       
            model.GetData(products, pageId, take, 3);
            model.Filter = filter ?? "";
            model.Take = take;
            model.PageId = pageId;

            model.products = await products
                .OrderByDescending(p => p.CreateDate)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(p => new ProductsForAdminQueryModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    ShortDescription = p.ShortDescription,
                    ImageName = p.ImageName,
                    CreateDate = p.CreateDate,
                    UpdateDate = p.UpdateDate,
                    Slug = p.Slug,
                    Active = p.Active,  
                    Weight = p.Weight,
                })
                .ToListAsync();

            return model;
        }
    }
}
