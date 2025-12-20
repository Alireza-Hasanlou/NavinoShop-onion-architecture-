using Shared.Domain.Enums;

namespace Site.Application.Contract.BanerService.Query
{
    public class BanerForUiQueryModel
    {
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
        public string Url { get; set; }
        public BanerState? BanerState { get; set; }
    }
}
