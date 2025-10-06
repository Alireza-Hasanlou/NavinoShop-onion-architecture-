using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Contract.CityApplication
{
    public interface ICityApplication
    {
        Task<OperationResult> Create(CreateCityModel command);
        Task<OperationResult> Edit(EditCityModel command);
        Task<bool> ChangeStatus(int id, CityStatus status);
        Task<bool> ExistTitleForCreate(string title, int stateid);
        Task<bool> ExistTitleForEdit(string title, int id, int stateid);
        Task<EditCityModel> GetCityForEdit(int id);
        Task<List<CityViewModel>> GetAllForState(int stateId);
    }
}
