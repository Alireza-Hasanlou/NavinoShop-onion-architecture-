using Shared.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Product.Command
{
    public interface IProductCommands
    {
        Task<OperationResult> CreateAsync(CreateProductCommandModel command);
        Task<OperationResult> EditAsync(EditProductCommandModel commandModel);
        Task<EditProductCommandModel> GetForEditAsync(int productId);
        Task<OperationResult> ChangeActivation(int productId);
    }
}