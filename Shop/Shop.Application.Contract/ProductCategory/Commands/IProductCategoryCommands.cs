using Shared.Application;
using Shop.Application.Contract.Product.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.ProductCategory.Commands
{
    public interface IProductCategoryCommands
    {
        Task<OperationResult> CreateAsync(CreateProductCategoryCommandModel command);
        Task<OperationResult> EditAsync(EditProductCategoryCommandModel command);
        Task<EditProductCategoryCommandModel> GetForEditAsync(int productCategoryId);
        Task<OperationResult> ChangeActivation(int productCategoryId);
    }
}
