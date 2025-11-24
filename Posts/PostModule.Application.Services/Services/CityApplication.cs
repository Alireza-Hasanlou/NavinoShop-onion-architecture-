using PostModule.Application.Contract.CityApplication;
using PostModule.Domain.CityEntity;
using PostModule.Domain.Services;
using Shared.Application;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using System.Threading.Tasks;


namespace PostModule.Application.Services
{
    internal class CityApplication : ICityApplication
    {
        private readonly ICityRepository _cityRepository;
        public CityApplication(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<bool> ChangeStatusAsync(int id, CityStatus status) =>
            await _cityRepository.ChangeStatus(id, status);



        public async Task<OperationResult> CreateAsync(CreateCityModel command)
        {
            if (await _cityRepository.ExistByAsync(c => c.Title == command.Title && c.StateId == command.StateId))
                return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
            City city = new(command.StateId, command.Title, CityStatus.شهرستان_معمولی);
            var result = await _cityRepository.CreateAsync(city);
            if (result.Success)
            {
                return new(true);
            }
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
        }

        public async Task<OperationResult> EditAsync(EditCityModel command)
        {
            if (await _cityRepository.ExistByAsync(c => c.Title == command.Title && c.StateId == command.StateId && c.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
            City city = await _cityRepository.GetByIdAsync(command.Id);
            city.Edit(command.Title, city.Status);
            if (await _cityRepository.SaveAsync()) return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
        }

        public async Task<bool> ExistTitleForCreateAsync(string title, int stateid) =>
            await _cityRepository.ExistByAsync(c => c.Title == title && c.StateId == stateid);



        public async Task<bool> ExistTitleForEditAsync(string title, int id, int stateid) =>
             await _cityRepository.ExistByAsync(c => c.Title == title && c.StateId == stateid && c.Id != id);




        public async Task<List<CityViewModel>> GetAllForStateAsync(int stateId) =>
          await _cityRepository.GetAllForState(stateId);

        public async Task<EditCityModel> GetCityForEditAsync(int id) =>
           await _cityRepository.GetCityForEdit(id);
    }
}
