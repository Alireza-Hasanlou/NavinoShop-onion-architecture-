using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shared.Domain.Enums;
using Site.Application.Contract.BanerService.Query;
using Site.Domain.BanerAgg;
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

    public async Task<List<BanerForUiQueryModel>> GetForUi(int count, BanerState state)
    {
        if (state == BanerState.بنر_تبلیغاتی_سمت_راست_وسط_410x100)
        {
            return _banerRepository.GetAllBy(b => b.State == state||b.State==BanerState.بنر_تبلیغاتی_سمت_چپ_وسط_850x100 && b.Active).Select(b => new BanerForUiQueryModel
            {
                ImageAlt = b.ImageAlt,
                ImageName = $"{FileDirectories.BanerImageDirectory}{b.ImageName}",
                Url = b.Url,
                BanerState = b.State

            }).Take(count).ToList();
        }
        else
        {
        return _banerRepository.GetAllBy(b => b.State == state && b.Active).Select(b => new BanerForUiQueryModel
        {
            ImageAlt = b.ImageAlt,
            ImageName = $"{FileDirectories.BanerImageDirectory}{b.ImageName}",
            Url = b.Url,
            BanerState = b.State
            
        }).Take(count).ToList();
        }


    }
}