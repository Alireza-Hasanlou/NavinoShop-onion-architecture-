using Shared.Domain.Enums;

namespace Site.Application.Contract.BanerService.Query
{
    public class BanerForAdminQueryModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
        public BanerState State { get; set; }
        public bool Active { get; set; }
        public string CreationDate { get; set; }
    }
}
