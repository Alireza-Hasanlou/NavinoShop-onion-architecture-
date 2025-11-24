using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.StateApplication;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.State
{
    public class CreateModel : PageModel
    {
        private readonly IStateApplication _stateApplication;

        public CreateModel(IStateApplication stateApplication)
        {
            _stateApplication = stateApplication;
        }
        [BindProperty]
        public CreateStateModel  CreateState{ get; set; }
        public void OnGet()
        {
           
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _stateApplication.CreateAsync(CreateState);
            if (result.Success)
            {
                TempData["success"] = "استان با موفقیت ایجاد شد";
                return Redirect("/Admin/Post/State/Index");
            }
            ModelState.AddModelError($"CreateState.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
