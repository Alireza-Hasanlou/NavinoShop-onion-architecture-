using Shared.Application;
using Shared.Application.Validations;
using Store.Application.Contract.Store.Command;
using Store.Domain.StoreAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Commands
{
    internal class StoreCommands : IStoreCommands
    {
        private readonly IStoreRepository _storeRepository;


        public StoreCommands(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<OperationResult> CreateAsync(CreateStoreCommandModel command)
        {
            if (await _storeRepository.ExistByAsync(x => x.SellerId == command.SellerId))
                return new(false, "برای این فروشگاه انبار موجود است ");
            Store.Domain.StoreAgg.Store store = new Domain.StoreAgg.Store(command.SellerId, command.Description, command.UserId);
            var res = await _storeRepository.CreateAsync(store);
            if (res.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> EditAsync(EditStoreCommandModel command)
        {
            var store = await _storeRepository.GetByIdAsync(command.Id);
            if (store == null)
                return new(false, "انباری با شناسه مورد نظر یافت نشد");
            store.EditDescription(command.Description);
            if (await _storeRepository.SaveAsync())
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<EditStoreCommandModel> GetForEditAsync(int Id)
        {
            var store = await _storeRepository.GetByIdAsync(Id);
            return new EditStoreCommandModel
            {
                Id = store.Id,
                Description = store.Description
            };
        }
    }
}
