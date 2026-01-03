using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shared.Domain.Enums;
using Site.Application.Contract.BanerService.Query;
using Site.Domain.BanerAgg;
using System.Reflection;
using System.Threading.Tasks;

namespace Site.Query.Services;

internal class BanerQuery : IBanerQueryService
{
    private readonly IBanerRepository _banerRepository;

    public BanerQuery(IBanerRepository banerRepository)
    {
        _banerRepository = banerRepository;
    }

    public async Task<List<BanerForAdminQueryModel>> GetAllForAdmin()
    {

        return _banerRepository.GetAll().Select(b => new BanerForAdminQueryModel
        {
            Active = b.Active,
            CreationDate = b.CreateDate.ToPersainDate(),
            Id = b.Id,
            ImageName = $"{FileDirectories.BanerImageDirectory100}{b.ImageName}",
            State = b.State,
            ImageAlt = b.ImageAlt,
        }).ToList();
    }

    public async Task<BanerForUiQueryModel> GetLeftSideBanerForBlog()
    {
        var baner = await _banerRepository.GetAllBy(a => a.Active
        && a.State == BanerState.بنر_تکی_بلاگ_سمت_چپ_280x230)
            .AsNoTracking()
            .Select(b => new BanerForUiQueryModel
            {
                ImageAlt = b.ImageAlt,
                ImageName = $"{FileDirectories.BanerImageDirectory}{b.ImageName}",
                Url = b.Url,
                BanerState = b.State
            }).SingleOrDefaultAsync();
        if (baner != null)
            return baner;
        return new();
    }
    public async Task<BanerForUiQueryModel> GetCenterBanerForBlog()
    {
        var baner = await _banerRepository.GetAllBy(a => a.Active
        && a.State == BanerState.بنرباریک_وسط_بلاگ_1020x130)
            .AsNoTracking()
            .Select(b => new BanerForUiQueryModel
            {
                ImageAlt = b.ImageAlt,
                ImageName = $"{FileDirectories.BanerImageDirectory}{b.ImageName}",
                Url = b.Url,
                BanerState = b.State
            }).SingleOrDefaultAsync();

        if (baner != null)
            return baner;
        return new();

    }
    public async Task<List<BanerForUiQueryModel>> GetForUi(int count, BanerState state)
    {
        if (state == BanerState.بنر_تبلیغاتی_سمت_راست_وسط_410x100)
        {
            return _banerRepository.GetAllBy(b => b.State == state
            || b.State == BanerState.بنر_تبلیغاتی_سمت_چپ_وسط_850x100
            && b.Active)
                .Take(count)
                .Select(b => new BanerForUiQueryModel
                {
                    ImageAlt = b.ImageAlt,
                    ImageName = $"{FileDirectories.BanerImageDirectory}{b.ImageName}",
                    Url = b.Url,
                    BanerState = b.State

                }).ToList();

        }
        else
        {
            return _banerRepository.GetAllBy(b => b.State == state && b.Active)
                .Take(count)
                .Select(b => new BanerForUiQueryModel
                {
                    ImageAlt = b.ImageAlt,
                    ImageName = $"{FileDirectories.BanerImageDirectory}{b.ImageName}",
                    Url = b.Url,
                    BanerState = b.State

                }).ToList();
        }


    }
}