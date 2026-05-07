using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Contract.StoreProduct.Command
{
    public interface IStoreProductCommands
    {
        Task<OperationResult> CreateAsync(CreateStoreProductCommandModel command);
        Task<OperationResult> DeleteAsync(int Id);
    }
}
