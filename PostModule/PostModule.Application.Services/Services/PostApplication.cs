using Shared.Application;
using PostModule.Application.Contract.PostApplication;
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
    internal class PostApplication : IPostApplication
    {
        private readonly IPostRepository _postRepository;
        public PostApplication(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<bool> ChangeActivationAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            post.ActivationChange();
            return await _postRepository.SaveAsync();
        }

        public async Task<OperationResult> CreateAsync(CreatePost command)
        {
            if (await _postRepository.ExistByAsync(p => p.Title == command.Title))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage,nameof(command.Title));
            Post post = new(command.Title, command.Status, command.TehranPricePlus, command.StateCenterPricePlus,
               command.CityPricePlus, command.InsideStatePricePlus, command.StateClosePricePlus,
               command.StateNonClosePricePlus,command.Description);
            var result = await _postRepository.CreateAsync(post);
            if (result.Success)
                return new(true);

            return new OperationResult(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
        }

        public async Task<OperationResult> EditAsync(EditPost command)
        {
            if (await _postRepository.ExistByAsync(p => p.Title == command.Title && p.Id != command.Id))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
            var post = await _postRepository.GetByIdAsync(command.Id);
             post.Edit(command.Title, command.Status, command.TehranPricePlus, command.StateCenterPricePlus,
               command.CityPricePlus, command.InsideStatePricePlus, command.StateClosePricePlus,
               command.StateNonClosePricePlus,command.Description);
            if (await _postRepository.SaveAsync())
                return new(true);

            return new OperationResult(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
        }

        public async Task<EditPost> GetForEditAsync(int id)
        {
            return await _postRepository.GetForEditAsync(id);
        }

        public async Task<bool> InsideCityChangeAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            post.InsideCityChange();
            return await _postRepository.SaveAsync();
        }

        public async Task<bool> OutSideCityChangeAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            post.OutSideCityChange();
            return await _postRepository.SaveAsync();
        }
    }
}
