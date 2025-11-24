using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostQuery;
using PostModule.Application.Contract.StateApplication;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.State
{
    public class EditModel : PageModel
    {
        private readonly IStateApplication _stateApplication;

        public EditModel(IStateApplication stateApplication)
        {
            _stateApplication = stateApplication;
        }
        [BindProperty]
        public EditStateModel EditState { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            if (Id < 1)
                return Page();

            EditState = await _stateApplication.GetStateForEditAsync(Id);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _stateApplication.CreateAsync(EditState);
            if (result.Success)
            {
                TempData["success"] = "استان با موفقیت ویرایش شد";
                return Redirect("/Admin/Post/State/Index");
            }
            ModelState.AddModelError($"EditState.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
