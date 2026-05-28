using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.Comments;
using Shared.Domain.Enums;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class ProductCommentsViewComponent : ViewComponent
    {
        private readonly ICommentsUiQueryService _commentsUiQueryService;

        public ProductCommentsViewComponent(ICommentsUiQueryService commentsUiQueryService)
        {
            _commentsUiQueryService = commentsUiQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int ProductId)
        {
            var comments = await _commentsUiQueryService.GetCommments(ProductId, CommentFor.محصول, 5);
            return View();  
        }
    }
}