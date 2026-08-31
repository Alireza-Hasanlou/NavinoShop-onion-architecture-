using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contract.UserAddressService.Query
{
    public interface IUserAddressQueryService
    {
        Task<UserAddressForOrderQueryModel> GetByIdAsync(int addressId);
        Task<UserAddressForOrderQueryModel> GetUserDefaultAddress(int UserId);
    }
    public class UserAddressForOrderQueryModel
    {
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string AddressDetail { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }

    }

}
