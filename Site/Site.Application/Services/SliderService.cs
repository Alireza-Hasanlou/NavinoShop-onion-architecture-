using Shared;
using Shared.Application;
using Shared.Application.Service;
using Shared.Application.Validations;
using Site.Application.Contract.SliderService.Command;
using Site.Domain.BanerAgg;
using Site.Domain.SliderAgg;
using System.Threading.Tasks;

namespace Site.Application.Services
{
    internal class SliderService : ISliderCommandService
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IFileService _fileService;

        public SliderService(ISliderRepository sliderRepository, IFileService fileService)
        {
            _sliderRepository = sliderRepository;
            _fileService = fileService;
        }

        public async Task<OperationResult> ActivationChangeAsync(int id)
        {
            var slider =await _sliderRepository.GetByIdAsync(id);
            slider.ActivationChange();
            if (await _sliderRepository.SaveAsync())
                return new(true);
            return new(false);
        }

        public async Task<OperationResult> CreateAsync(CreateSliderCommandModel command)
        {
            if (command.ImageFile == null || !command.ImageFile.IsImage())
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

            string imageName =await _fileService.UploadImage(command.ImageFile, FileDirectories.SliderImageFolder);
            if (imageName == "")
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

            _fileService.ResizeImage(imageName, FileDirectories.SliderImageFolder, 100);
            Slider slider = new(imageName, command.ImageAlt,command.Url);
            var result = await _sliderRepository.CreateAsync(slider);
            if (result.Success) return new(true);
            _fileService.DeleteImage($"{FileDirectories.SliderImageDirectory}{imageName}");
            _fileService.DeleteImage($"{FileDirectories.SliderImageDirectory100}{imageName}");
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.ImageAlt));
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var baner = await _sliderRepository.GetByIdAsync(id);
            if (baner != null)
            {
                var result = await _sliderRepository.DeleteAsync(baner);
                if (result.Success)
                    return new(true);
            }

            return new(false, ValidationMessages.SystemErrorMessage, nameof(baner.ImageAlt));
        }
        public async Task<OperationResult> EditAsync(EditSliderCommandModel command)
        {
            var slider =await _sliderRepository.GetByIdAsync(command.Id);
            string imageName = slider.ImageName;
            string oldImageName = slider.ImageName;
            if (command.ImageFile != null)
            {
                if (!command.ImageFile.IsImage()) return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));
                imageName =await _fileService.UploadImage(command.ImageFile, FileDirectories.SliderImageFolder);
                if (imageName == "")
                    return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));
                _fileService.ResizeImage(imageName, FileDirectories.SliderImageFolder, 100);
            }
            slider.Edit(imageName, command.ImageAlt,command.Url);
            if (await _sliderRepository.SaveAsync())
            {
                if (command.ImageFile != null)
                {
                    _fileService.DeleteImage($"{FileDirectories.SliderImageDirectory}{oldImageName}");
                    _fileService.DeleteImage($"{FileDirectories.SliderImageDirectory100}{oldImageName}");
                }
                return new(true);
            }
            else
            {

                if (command.ImageFile != null)
                {
                    _fileService.DeleteImage($"{FileDirectories.SliderImageDirectory}{imageName}");
                    _fileService.DeleteImage($"{FileDirectories.SliderImageDirectory100}{imageName}");
                }
                return new(false, ValidationMessages.SystemErrorMessage, nameof(command.ImageAlt));
            }
        }

        public async Task<EditSliderCommandModel> GetForEditAsync(int id) =>
            await _sliderRepository.GetForEditAsync(id);
    }
}
