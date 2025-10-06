using PostModule.Application.Contract.StateApplication;
using PostModule.Domain.Services;
using PostModule.Domain.StateEntity;
using Shared.Application;
using Shared.Application.Validations;
using System.Threading.Tasks;

namespace PostModule.Application.Services
{
    internal class StateApplication : IStateApplication
    {
        private readonly IStateRepository _stateRepository;
        public StateApplication(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<bool> ChangeStateClose(int id, List<int> stateCloses)
        {
            if (stateCloses.Count() < 1) return false;
            var state = await _stateRepository.GetByIdAsync(id);
            state.ChangeCloseStates(stateCloses);
            return await _stateRepository.SaveAsync();
        }

        public async Task<OperationResult> Create(CreateStateModel command)
        {
            if (await _stateRepository.ExistByAsync(s => s.Title == command.Title))
                return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
            State state = new(command.Title);
            var result = await _stateRepository.CreateAsync(state);
            if (result.Success) return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
        }

        public async Task<OperationResult> Edit(EditStateModel command)
        {
            if (await _stateRepository.ExistByAsync(s => s.Title == command.Title && s.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
            State state = await _stateRepository.GetByIdAsync(command.Id);
            state.Edit(command.Title);
            if (await _stateRepository.SaveAsync()) return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
        }

        public async Task<bool> ExistTitleForCreate(string title) =>
           await _stateRepository.ExistByAsync(s => s.Title == title);

        public async Task<bool> ExistTitleForEdit(string title, int id) =>
           await _stateRepository.ExistByAsync(s => s.Title == title && s.Id != id);

        public async Task<List<StateViewModel>> GetAll() =>
           await _stateRepository.GetAllStateViewModel();

        public async Task<EditStateModel> GetStateForEdit(int id)
        {
            return await _stateRepository.GetStateForEdit(id);
        }
    }
}
