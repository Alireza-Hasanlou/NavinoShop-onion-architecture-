using Shared.Application;
using Site.Domain.SiteServiceAgg;
using Shared;
using Site.Application.Contract.SiteServiceService.Command;
using Shared.Application.Service;
using Shared.Application.Validations;

namespace Site.Application.Services;

internal class SiteServiceService : ISiteServiceCommandService
{
    private readonly ISiteServiceRepository _siteServiceepository;
    private readonly IFileService _fileService;

    public SiteServiceService(ISiteServiceRepository siteServiceepository, IFileService fileService)
    {
        _siteServiceepository = siteServiceepository;
        _fileService = fileService;
    }

    public async Task<OperationResult> ActivationChange(int id)
    {
        var service = await _siteServiceepository.GetByIdAsync(id);
        service.ActivationChange();
        if( await _siteServiceepository.SaveAsync())
            return new(true);
        return new(false);
    }

    public async Task<OperationResult> Create(CreateSiteServiceCommnadModel commmand)
    {
        if (commmand.ImageFile == null || !commmand.ImageFile.IsImage())
            return new(false, ValidationMessages.ImageErrorMessage, nameof(commmand.ImageFile));

        string imageName = await _fileService.UploadImage(commmand.ImageFile, FileDirectories.ServiceImageFolder);
        if (imageName == "")
            return new(false, ValidationMessages.ImageErrorMessage, nameof(commmand.ImageFile));

        _fileService.ResizeImage(imageName, FileDirectories.ServiceImageFolder, 100);
        SiteService service = new(imageName, commmand.ImageAlt, commmand.Title);
        var result = await _siteServiceepository.CreateAsync(service);
        if (result.Success)
            return new(true);
        _fileService.DeleteImage($"{FileDirectories.ServiceImageDirectory}{imageName}");
        _fileService.DeleteImage($"{FileDirectories.ServiceImageDirectory100}{imageName}");
        return new(false, ValidationMessages.SystemErrorMessage, nameof(commmand.ImageAlt));
    }

    public async Task<OperationResult> Edit(EditSiteServiceCommandModel commmand)
    {
        var service =await _siteServiceepository.GetByIdAsync(commmand.Id);
        string imageName = service.ImageName;
        string oldImageName = service.ImageName;
        if (commmand.ImageFile != null)
        {
            if (!commmand.ImageFile.IsImage()) return new(false, ValidationMessages.ImageErrorMessage, nameof(commmand.ImageFile));
            imageName = await _fileService.UploadImage(commmand.ImageFile, FileDirectories.ServiceImageFolder);
            if (imageName == "")
                return new(false, ValidationMessages.ImageErrorMessage, nameof(commmand.ImageFile));
            _fileService.ResizeImage(imageName, FileDirectories.ServiceImageFolder, 100);
        }
        service.Edit(imageName, commmand.ImageAlt, commmand.Title);
        if (await _siteServiceepository.SaveAsync())
        {
            if (commmand.ImageFile != null)
            {
                _fileService.DeleteImage($"{FileDirectories.ServiceImageDirectory}{oldImageName}");
                _fileService.DeleteImage($"{FileDirectories.ServiceImageDirectory100}{oldImageName}");
            }
            return new(true);
        }
        else
        {

            if (commmand.ImageFile != null)
            {
                _fileService.DeleteImage($"{FileDirectories.ServiceImageDirectory}{imageName}");
                _fileService.DeleteImage($"{FileDirectories.ServiceImageDirectory100}{imageName}");
            }
            return new(false, ValidationMessages.SystemErrorMessage, nameof(commmand.ImageAlt));
        }

    }

    public async Task<EditSiteServiceCommandModel> GetForEdit(int id) =>
       await _siteServiceepository.GetForEdit(id);
}
