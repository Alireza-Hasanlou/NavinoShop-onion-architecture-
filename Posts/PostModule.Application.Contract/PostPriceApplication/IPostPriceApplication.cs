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
        Task<OperationResult> Create(CreatePostPrice command);
        Task<OperationResult> Edit(EditPostPrice command);
        Task<EditPostPrice> GetForEdit(int id);
    }
}
