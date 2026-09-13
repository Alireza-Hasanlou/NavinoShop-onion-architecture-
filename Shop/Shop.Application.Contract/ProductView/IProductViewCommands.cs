using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.ProductView
{
    public interface IProductViewCommands
    {
        Task CreateAsync(CreateProductViewCommandModel model);
        Task<bool> IsExistProductViewAsync(int productId, string sessionId);
    }
}
