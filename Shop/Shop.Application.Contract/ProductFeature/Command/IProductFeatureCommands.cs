using Shared.Application;
using Shop.Application.Contract.Product.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.ProductFeature.Command
{
    public interface IProductFeatureCommands
    {
        Task<OperationResult> CreateAsync(CreateProductFeatureCommandModel command);
        Task<OperationResult> EditAsync(EditProductFeatureCommandModel command);
        Task<OperationResult> DeleteAsync(int FeatureId);
        Task<EditProductFeatureCommandModel> GetForEditAsync(int FeatureId);

    }
}
