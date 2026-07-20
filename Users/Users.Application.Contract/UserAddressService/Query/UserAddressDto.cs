using Users.Application.Contract.UserAddressService.Command;

namespace Users.Application.Contract.UserAddressService.Query
{
    public class UserAddressDto:CreateUserAddressCommand
    {
        public int Id { get; set; }
        public string  CityName { get; set; }
        public string StateName { get; set; }

    }
}