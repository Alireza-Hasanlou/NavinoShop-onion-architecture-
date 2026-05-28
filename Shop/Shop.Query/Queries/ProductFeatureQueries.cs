using Microsoft.EntityFrameworkCore;
using Shop.Application.Contract.ProductFeature.Query;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductFreatureAgg;

namespace Shop.Query.Queries
{
    internal class ProductFeatureQueries : IProductFeatureQueries
    {
        private readonly IProductFeatureRepository _productFeatureRepository;
        private readonly IProductRepository _productRepository;

        public ProductFeatureQueries(IProductFeatureRepository productFeatureRepository, IProductRepository productRepository)
        {
            _productFeatureRepository = productFeatureRepository;
            _productRepository = productRepository;
        }

        public async Task<ProductFeatureAdminPage> GetProdutFeaturesForAdmin(int productId)
        {
            var model = new ProductFeatureAdminPage();
            if (productId < 1)
                return model;

            var product = await _productRepository.GetByIdAsync(productId);
            model.Title = $"ویژگی های محصول {product.Title}";
            model.ProductId = productId;
            model.ProductFeatures = await _productFeatureRepository.GetAllBy(p => p.ProductId == productId)
                .OrderByDescending(i => i.Id)
                .Select(f => new ProductFeatureQueryModel
                {
                    Id = f.Id,
                    Title = f.Title,
                    Value = f.Value
                }).ToListAsync();

            return model;
        }
    }
}
