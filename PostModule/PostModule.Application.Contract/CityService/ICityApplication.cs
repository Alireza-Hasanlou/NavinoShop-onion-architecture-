using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Contract.CityService
{
    public interface ICityCommandService
    {
        Task<OperationResult> CreateAsync(CreateCityModel command);
        Task<OperationResult> EditAsync(EditCityModel command);
        Task<bool> ChangeStatusAsync(int id, CityStatus status);
        Task<bool> ExistTitleForCreateAsync(string title, int stateid);
        Task<bool> ExistTitleForEditAsync(string title, int id, int stateid);
        Task<EditCityModel> GetCityForEditAsync(int id);
        Task<List<CityViewModel>> GetAllForStateAsync(int stateId);
    }
}
