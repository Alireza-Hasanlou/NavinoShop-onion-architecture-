using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Application.Contract.SliderService.Command
{
    public interface ISliderCommandService
    {
        Task<OperationResult> CreateAsync(CreateSliderCommandModel command);
        Task<OperationResult> EditAsync(EditSliderCommandModel command);
        Task<OperationResult> DeleteAsync(int id);
        Task<OperationResult> ActivationChangeAsync(int id);
        Task<EditSliderCommandModel> GetForEditAsync(int id);
    }
}
