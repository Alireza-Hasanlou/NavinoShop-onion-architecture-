using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.ProductGallery.Query
{
    public interface IProductGalleryQueries
    {
        Task<ProductGalleryAdminPaging> GetProductGalleriesForAdmin();
    }
}
