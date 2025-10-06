using Shared.Application;
using PostModule.Application.Contract.PostPriceApplication;
using PostModule.Domain.PostEntity;
using PostModule.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Application.Validations;

namespace PostModule.Application.Services
{
    internal class PostPriceApplication : IPostPriceApplication
    {
        private readonly IPostPriceRepository _postPriceRepository;
        public PostPriceApplication(IPostPriceRepository postPriceRepository)
        {
            _postPriceRepository = postPriceRepository;
        }

        public async Task<OperationResult> Create(CreatePostPrice command)
        {
            PostPrice postPrice = new(command.PostId, command.Start, command.End, command.TehranPrice,
                command.StateCenterPrice, command.CityPrice, command.InsideStatePrice,
                command.StateClosePrice, command.StateNonClosePrice);
            var result = await _postPriceRepository.CreateAsync(postPrice);
            if (result.Success)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Start));
        }

        public async Task<OperationResult> Edit(EditPostPrice command)
        {
            var postPrice = await _postPriceRepository.GetByIdAsync(command.Id);
            postPrice.Edit(command.Start, command.End, command.TehranPrice,
                command.StateCenterPrice, command.CityPrice, command.InsideStatePrice,
                command.StateClosePrice, command.StateNonClosePrice);
            if (await _postPriceRepository.SaveAsync())
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Start));
        }

        public async Task<EditPostPrice> GetForEdit(int id)
        {
            return await _postPriceRepository.GetForEdit(id);
        }
    }
}
