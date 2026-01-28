using Shared.Domain;
using PostModule.Domain.CityEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain.Enums;
using PostModule.Application.Contract.CityService;

namespace PostModule.Domain.Services
{
    public interface ICityRepository : IGenericRepository<City, int>
    {
		Task<bool> ChangeStatus(int id, CityStatus status);
		Task<List<CityViewModel>> GetAllForState(int stateId);
        Task<EditCityModel> GetCityForEdit(int id);
        Task<string> GetCityTitle(int cityId);
    }
}
