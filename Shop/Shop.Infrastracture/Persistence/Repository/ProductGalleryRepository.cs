using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Domain.ProductGalleryAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class ProductGalleryRepository : GenericRepository<ProductGallery, int>, IProductGalleryRepository
    {
        public ProductGalleryRepository(ShopContext context) : base(context)
        {
        }
    }
}
