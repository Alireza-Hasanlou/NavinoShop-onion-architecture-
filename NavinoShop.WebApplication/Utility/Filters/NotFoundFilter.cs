namespace NavinoShop.WebApplication.Utility.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class NotFoundFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
          
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
            if (context.Result is NotFoundResult || context.Result is NotFoundObjectResult)
            {
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/NotFound.cshtml",
                    StatusCode = 404
                };
            }
        }
    }
}
