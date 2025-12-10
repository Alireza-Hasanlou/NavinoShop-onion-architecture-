using Shared.Domain.Enums;


namespace PostModule.Application.Contract.CityService
{
    public class CityViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public CityStatus Status { get; set; }
        public string CreateDate { get; set; }
    }
}
