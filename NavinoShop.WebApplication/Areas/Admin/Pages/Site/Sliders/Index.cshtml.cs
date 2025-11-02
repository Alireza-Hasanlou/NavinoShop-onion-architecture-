using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SliderService.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Sliders
{
    public class IndexModel : PageModel
    {
        private readonly ISliderQueryservice _sliderQueryService;

        public IndexModel(ISliderQueryservice sliderQueryService)
        {
            _sliderQueryService = sliderQueryService;
        }
        public List<SliderForAdminQueryModel> sliders { get; set; }
        public async Task OnGet()
        {
            sliders = await _sliderQueryService.GetAllForAdmin();
        }
    }
}
