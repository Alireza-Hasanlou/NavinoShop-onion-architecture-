using Shared.Application;
using Shared;
using Site.Domain.SiteSettingAgg;
using Site.Application.Contract.SiteServiceService.Command;
using Shared.Application.Service;
using Site.Application.Contract.SiteSettingService.Command;
using Shared.Application.Validations;

namespace Site.Application.Services;

internal class SiteSettingService : ISiteSettingService
{
    private readonly ISiteSettingRepository _siteSettingRepository;
    private readonly IFileService _fileService;

    public SiteSettingService(ISiteSettingRepository siteSettingRepository, IFileService fileService)
    {
        _siteSettingRepository = siteSettingRepository;
        _fileService = fileService;
    }

    public async Task<UpsertSiteSetting> GetForUpsert() =>
      await  _siteSettingRepository.GetForUpsert();

    public async Task<OperationResult> Upsert(UpsertSiteSetting command)
    {
        SiteSetting site = await _siteSettingRepository.GetSingle();
        string logoName = site.LogoName;
        string oldLogoName = site.LogoName;
        if (command.LogoFile != null)
        {
            if (!command.LogoFile.IsImage()) return new(false, ValidationMessages.ImageErrorMessage, nameof(command.LogoFile));
            logoName =await _fileService.UploadImage(command.LogoFile, FileDirectories.SiteImageFolder);
            if (logoName == "")
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.LogoFile));
            _fileService.ResizeImage(logoName, FileDirectories.SiteImageFolder, 64);
        }
        string AboutUsImageName = site.AboutImageName;
        string oldAboutUsImageName = site.AboutImageName;
        if (command.AboutUsImageFile != null)
        {
            if (!command.AboutUsImageFile.IsImage()) return new(false, ValidationMessages.ImageErrorMessage, nameof(command.AboutUsImageFile));
            AboutUsImageName = await _fileService.UploadImage(command.AboutUsImageFile, FileDirectories.SiteImageFolder);
            if (AboutUsImageName == "")
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.AboutUsImageFile));
        }
        string favIconName = site.FavIcon;
        string oldfavIconName = site.FavIcon;
        if (command.FavIconFile != null)
        {
            if (!command.FavIconFile.IsImage()) return new(false, ValidationMessages.ImageErrorMessage, nameof(command.FavIconFile));
            favIconName = await _fileService.UploadImage(command.FavIconFile, FileDirectories.SiteImageFolder);
            if (logoName == "")
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.FavIconFile));
            _fileService.ResizeImage(favIconName, FileDirectories.SiteImageFolder, 64);
            _fileService.ResizeImage(favIconName, FileDirectories.SiteImageFolder, 32);
            _fileService.ResizeImage(favIconName, FileDirectories.SiteImageFolder, 16);
        }
        site.Edit(command.Instagram, command.WhatsApp, command.Telegram, command.Youtube, logoName,
            command.LogoAlt, favIconName, command.Enamad, command.SamanDehi, command.SeoBox,
            command.Android, command
            .IOS, command.FooterDescription, command.FooterTitle, command.Phone1, command.Phone2,
            command.Email1, command.Email2, command.Address, command.ContactDescription, command.AboutDescription, command.AboutTitle, AboutUsImageName);
        if ( await _siteSettingRepository.SaveAsync())
        {
            if(command.LogoFile != null)
            {
                if (!string.IsNullOrEmpty(oldLogoName))
                {
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory}{oldLogoName}");
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory300}{oldLogoName}");
                }
            }
            if (command.FavIconFile != null)
            {
                if (!string.IsNullOrEmpty(oldfavIconName))
                {
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory}{oldfavIconName}");
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory64}{oldfavIconName}");
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory32}{oldfavIconName}");
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory16}{oldfavIconName}");
                }
            }
            if (command.AboutUsImageFile != null)
            {
                if (!string.IsNullOrEmpty(oldAboutUsImageName))
                {
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory}{oldAboutUsImageName}");;
             
                }
            }
            return new(true);
        }
        else
        {
            if (command.LogoFile != null)
            {
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory}{logoName}");
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory300}{logoName}");
                
            }
            if (command.FavIconFile != null)
            {
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory}{favIconName}");
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory64}{favIconName}");
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory32}{favIconName}");
                    _fileService.DeleteImage($"{FileDirectories.SiteImageDirectory16}{favIconName}");
            }
            return new(false,ValidationMessages.SystemErrorMessage,nameof(command.Instagram));
        }
    }
}