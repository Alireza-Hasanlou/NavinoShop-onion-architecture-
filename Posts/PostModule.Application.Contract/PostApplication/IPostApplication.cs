using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Contract.PostApplication
{
    public interface IPostApplication
    {
        Task<OperationResult> CreateAsync(CreatePost command);
        Task<OperationResult> EditAsync(EditPost command);
        Task<EditPost> GetForEditAsync(int id);
        Task<bool> ChangeActivationAsync(int id);
        Task<bool> InsideCityChangeAsync(int id);
        Task<bool> OutSideCityChangeAsync(int id);
    }
}
