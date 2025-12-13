using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Contract.PostPriceApplication
{
    public interface IPostPriceApplication
    {
        Task<OperationResult> CreateAsync(CreatePostPrice command);
        Task<OperationResult> EditAsync(EditPostPrice command);
        Task<EditPostPrice> GetForEditAsync(int id);
    }
}
