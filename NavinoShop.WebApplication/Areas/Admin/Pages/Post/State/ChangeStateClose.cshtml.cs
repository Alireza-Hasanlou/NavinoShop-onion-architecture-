using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.StateApplication;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.State
{
    public class ChangeStateCloseModel : PageModel
    {
        private readonly IStateApplication _stateApplication;

        public ChangeStateCloseModel(IStateApplication stateApplication)
        {
            _stateApplication = stateApplication;
        }

        public async Task<IActionResult> OnGet(int id, List<int>? stateCloses)
        {
            if (stateCloses.Count() < 1)
            {
                TempData["ChooseState"] = "لطفا حداقل یک استان همجوار انتخاب کنید";
                return Redirect($"/Admin/Post/City/Index?id={id}");
            }
            if (await _stateApplication.ChangeStateCloseAsync(id, stateCloses))
            {
                TempData["success"] = "استان های همجوار با باموفقیت تغییر کرد";
                return Redirect($"/Admin/Post/City/Index?id={id}");
            }
            else
            {
                TempData["faild"] = "عملیات با خطا مواجه شد ";
                return Redirect($"/Admin/Post/City/Index?id={id}");
            }
        }
      
    }
}
