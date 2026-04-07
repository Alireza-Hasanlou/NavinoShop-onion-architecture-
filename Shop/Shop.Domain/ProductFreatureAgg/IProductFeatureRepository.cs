using Shared.Domain;
using Shop.Application.Contract.ProductFeature.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ProductFreatureAgg
{
    public interface IProductFeatureRepository : IGenericRepository<ProductFreature, int>
    {
        Task<EditProductFeatureCommandModel> GetForEditAsync(int featureId);
    }
}
