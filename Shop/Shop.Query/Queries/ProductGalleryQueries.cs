using Microsoft.EntityFrameworkCore;
using Shop.Application.Contract.ProductGallery.Query;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductGalleryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Query.Queries
{
    internal class ProductGalleryQueries : IProductGalleryQueries
    {
        private readonly IProductGalleryRepository _productGalleryRepository;
        private readonly IProductRepository _productRepository;

        public ProductGalleryQueries(IProductGalleryRepository productGalleryRepository, IProductRepository productRepository)
        {
            _productGalleryRepository = productGalleryRepository;
            _productRepository = productRepository;
        }

        public async Task<ProductGalleryAdminPage> GetProductGalleriesForAdmin(int productId)
        {
            var model = new ProductGalleryAdminPage();
            if (productId < 1)
                return model;
            var product = await _productRepository.GetByIdAsync(productId);
            model.ProductId = productId;
            model.Title = $"نمایش گالری تصاویر {product.Title}";
            model.ProductGalleries = await _productGalleryRepository.GetAllBy(p => p.ProductId == productId)
                .OrderByDescending(i => i.Id)
                .Select(g => new ProductGalleryForAdminQueryModel
                {
                    Id = g.Id,
                    ImageAlt = g.ImageAlt,
                    ImageName = g.ImageName,
                })
                .ToListAsync();


            return model;
        }
    }
}
