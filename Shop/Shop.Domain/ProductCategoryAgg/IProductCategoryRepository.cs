using Shared.Domain;
using Shop.Application.Contract.ProductCategory.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ProductCategoryAgg
{
    public interface IProductCategoryRepository : IGenericRepository<ProductCategory, int>
    {
        Task<EditProductCategoryCommandModel> GetForEditAsync(int productCategoryId);
    }
}
