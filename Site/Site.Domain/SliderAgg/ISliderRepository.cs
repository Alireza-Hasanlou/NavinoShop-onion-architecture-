using Shared.Domain;
using Site.Application.Contract.SliderService.Command;

namespace Site.Domain.SliderAgg
{
    public interface ISliderRepository : IGenericRepository<Slider, int>
    {
       Task< EditSliderCommandModel> GetForEdit(int id);
    }
}
