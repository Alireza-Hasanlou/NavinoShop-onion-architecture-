
using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Site.Application.Contract.SliderService.Command;
using Site.Domain.SliderAgg;
using Site.Infrastructure.Persistence.Context;

namespace Site.Infrastructure.Persistence.Repository;

internal class SliderRepository : GenericRepository<Slider, int>, ISliderRepository
{
    private readonly SiteContext _context;

    public SliderRepository(SiteContext context) : base(context)
    {
        _context = context;
    }

    public async Task<EditSliderCommandModel> GetForEdit(int id) =>
       await _context.Sliders.Select(s => new EditSliderCommandModel
        {
            ImageAlt = s.ImageAlt,
            Id = s.Id,
            ImageFile = null,
            ImageName = s.ImageName,
            Url = s.Url
        }).SingleOrDefaultAsync(s => s.Id == id);
}
