using Shared.Application;
using Shop.Application.Contract.Product.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.ProductGallery.Command
{
    public interface IProductGalleryCommands
    {
        Task<OperationResult> CreateAsync(CreateProductGalleryCommandModel command);
        Task<OperationResult> DeleteAsync(int Id);
    }


}
