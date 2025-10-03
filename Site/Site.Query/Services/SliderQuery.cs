using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Site.Application.Contract.SliderService.Query;
using Site.Domain.SliderAgg;
using System.Threading.Tasks;

namespace Site.Query.Services;

internal class SliderQuery : ISliderQueryservice
{
    private readonly ISliderRepository _sliderRepository;

    public SliderQuery(ISliderRepository sliderRepository)
    {
        _sliderRepository = sliderRepository;
    }

    public async Task<List<SliderForAdminQueryModel>> GetAllForAdmin()
    {

        return await _sliderRepository.GetAll().Select(s => new SliderForAdminQueryModel
        {
            Active = s.Active,
            ImageAlt = s.ImageAlt,
            CreationDate = s.CreateDate.ToPersainDate(),
            Id = s.Id,
            ImageName = FileDirectories.SliderImageDirectory100 + s.ImageName
        }).ToListAsync();

    }

    public async Task<List<SliderForUiQueryModel>> GetAllForUi()
    {

        return
             await _sliderRepository.GetAllBy(s => s.Active).Select(s => new SliderForUiQueryModel()
             {
                 ImageAlt = s.ImageAlt,
                 ImageName = FileDirectories.SliderImageDirectory + s.ImageName,
                 Url = s.Url
             }).ToListAsync();
    }
}
