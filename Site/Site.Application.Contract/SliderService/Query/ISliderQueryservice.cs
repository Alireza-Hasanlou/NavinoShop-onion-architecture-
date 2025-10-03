using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Application.Contract.SliderService.Query
{
    public interface ISliderQueryservice
    {
       Task< List<SliderForAdminQueryModel>> GetAllForAdmin();
       Task< List<SliderForUiQueryModel>> GetAllForUi();
    }
}
