using Shared.Domain;
using Shop.Application.Contract.ProductFeature.Command;


namespace Shop.Domain.ProductFreatureAgg
{
    public interface IProductFeatureRepository : IGenericRepository<ProductFreature, int>
    {
        Task<EditProductFeatureCommandModel> GetForEditAsync(int featureId);
    }
}
