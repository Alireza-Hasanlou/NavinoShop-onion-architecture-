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
        Task<OperationResult> Create(CreateSliderCommandModel command);
        Task<OperationResult> Edit(EditSliderCommandModel command);
        Task<OperationResult> ActivationChange(int id);
        Task<EditSliderCommandModel> GetForEdit(int id);
    }
}
