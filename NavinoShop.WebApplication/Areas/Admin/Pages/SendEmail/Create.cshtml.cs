using Emails.Application.Contract.SendEmailService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace NavinoShop.WebApplication.Areas.Admin.Pages.SendEmail
{

    public class CreateModel : PageModel
    {
        private readonly ISendEmailCommandService _sendEmailCommandService;

        public CreateModel(ISendEmailCommandService sendEmailCommandService)
        {
            _sendEmailCommandService = sendEmailCommandService;
        }

        [BindProperty]
        public CreateSendEmailCommnadModel CreateSendEmail { get; set; }
        public void OnGet()=> Page();
 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                Page();
          
            var result = await _sendEmailCommandService.CreateAsync(CreateSendEmail);
            if (result.Success)
            {
                TempData["Success"] = "خبرنامه جدید با موفقیت ایجاد شد";
                return RedirectToPage("Index");
            }
            ModelState.AddModelError($"CreateSendEmail.{result.ModelName}", result.Message);
            return Page();
        }


    }
}
