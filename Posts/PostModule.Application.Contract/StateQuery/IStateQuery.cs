using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Contract.StateQuery
{
    public interface ICityQuery
    {
        Task<List<StateQueryModel>> GetStatesWithCity();
        Task<List<StateAdminQueryModel>> GetStatesForAdmin();
        Task<StateDetailQueryModel> GetStateDetail(int id);
        Task<string> GetStateTitle(int id);
        Task<List<StateForChooseQueryModel>> GetStatesForChoose();
        Task<List<CityForChooseQueryModel>> GetCitiesForChoose(int stateId);
        Task<bool> IsStateCorrect(int stateId);
        Task<bool> IsCityCorrect(int stateId, int cityId);
    }
}
