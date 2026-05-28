using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.ProductFeature.Query
{
    public interface IProductFeatureQueries
    {
        Task<ProductFeatureAdminPage>GetProdutFeaturesForAdmin(int productId);  
        

    }
}
