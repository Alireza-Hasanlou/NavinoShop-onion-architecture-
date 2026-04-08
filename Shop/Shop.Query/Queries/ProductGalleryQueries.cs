using Shop.Application.Contract.ProductGallery.Query;
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

        public ProductGalleryQueries(IProductGalleryRepository productGalleryRepository)
        {
            _productGalleryRepository = productGalleryRepository;
        }

        public Task<ProductGalleryAdminPaging> GetProductGalleriesForAdmin()
        {
            throw new NotImplementedException(); 
        }
    }
}
