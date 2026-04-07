using Shared;
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

    public class ProductGalleryAdminPaging : BasePaging
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public List<ProductGalleryForAdminQueryModel> ProductGalleries { get; set; }

    }

    public class ProductGalleryForAdminQueryModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }

    }
}
