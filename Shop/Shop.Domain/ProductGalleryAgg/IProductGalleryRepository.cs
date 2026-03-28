using Shared.Domain;
using Shop.Domain.ProductAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ProductGalleryAgg
{
    public interface IProductGalleryRepository:IGenericRepository<ProductGallery, int >
    {
    }
}
