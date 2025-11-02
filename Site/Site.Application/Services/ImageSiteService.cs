using Shared;
using Shared.Application;
using Shared.Application.Service;
using Shared.Application.Validations;
using Site.Application.Contract.ImageSiteService.Command;
using Site.Domain.SiteImageAgg;

namespace Site.Application.Services;

internal class ImageSiteService : IImageSiteCommandService
{
    private readonly IImageSiteRepository _imageSiteRepository;
    private readonly IFileService _fileService;

    public ImageSiteService(IImageSiteRepository imageSiteRepository, IFileService fileService)
    {
        _imageSiteRepository = imageSiteRepository;
        _fileService = fileService;
    }

    public async Task<OperationResult> CreateAsync(CreateImageSiteCommandModel command)
    {
        if (command.ImageFile == null || !command.ImageFile.IsImage())
            return new(false, ValidationMessages.ImageErrorMessage, nameof(command.Title));
        string imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.ImageFolder);
        if (imageName == "")
            return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

        _fileService.ResizeImage(imageName, FileDirectories.ImageFolder, 100);

        SiteImage image = new(imageName, command.Title);
        var result = await _imageSiteRepository.CreateAsync(image);
        if (result.Success)
            return new(true);
        _fileService.DeleteImage($"{FileDirectories.ImageDirectory}{imageName}");
        _fileService.DeleteImage($"{FileDirectories.ImageDirectory100}{imageName}");
        return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
    }

    public async Task<OperationResult> DeleteFromDataBase(int id)
    {
        var image = await _imageSiteRepository.GetByIdAsync(id);
        string imageName = image.ImageName;
       var result= await _imageSiteRepository.DeleteAsync(image);

        if (result.Success)
        {
            _fileService.DeleteImage($"{FileDirectories.ImageDirectory}{imageName}");
            _fileService.DeleteImage($"{FileDirectories.ImageDirectory100}{imageName}");
            return new(true);
        }
        return new(false, ValidationMessages.SystemErrorMessage);
    }
}