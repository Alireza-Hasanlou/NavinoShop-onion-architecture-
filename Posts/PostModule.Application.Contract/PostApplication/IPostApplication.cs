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
        Task<OperationResult> Create(CreatePost command);
        Task<OperationResult> Edit(EditPost command);
        Task<EditPost> GetForEdit(int id);
        Task<bool> ActivationChange(int id);
        Task<bool> InsideCityChange(int id);
        Task<bool> OutSideCityChange(int id);
    }
}
