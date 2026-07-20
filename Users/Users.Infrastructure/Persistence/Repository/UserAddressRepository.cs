using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shared.Insfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserAddressService.Query;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using Users.Infrastructure.Persistence.Context;

namespace Users.Infrastructure.Persistence.Repository
{
    internal class UserAddressRepository : GenericRepository<UserAddress, int>, IUserAddressRepository
    {
        private readonly UserContext _context;
        public UserAddressRepository(UserContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserAddressDto> GetAddressForEditAsync(int id)
        {
            return await _context.UserAddresses.Where(i => i.Id == id)
                .Select(a => new UserAddressDto
                {
                    Id = a.Id,
                    AddressDetail = a.AddressDetail,
                    CityId = a.CityId,
                    FullName = a.FullName,
                    NationalCode = a.NationalCode,
                    Phone = a.Phone,
                    PostalCode = a.PostalCode,
                    StateId = a.StateId,

                }).SingleOrDefaultAsync();
        }

        public async Task<UserAddress> GetDefaultAddressAsync(int userId)
        {
            return await _context.UserAddresses.SingleOrDefaultAsync(x => x.UserId == userId && x.Is_Default);
        }

        public async Task<int> GetUserAddressCount()
        {
            return await _context.UserAddresses.CountAsync();
        }

        public async Task<OperationResult> SetAddressToDefaultAsync(int userId, int addressId)
        {
            try
            {
                var DefaultAddress = await _context.UserAddresses.SingleOrDefaultAsync(x => x.UserId == userId && x.Is_Default == true);
                if (DefaultAddress is not null)
                    DefaultAddress.ChangeDefultStatus(false);

                var newDefaultAddress = await _context.UserAddresses.SingleOrDefaultAsync(x => x.UserId == userId && x.Id == addressId);
                if (newDefaultAddress is not null)
                    newDefaultAddress.ChangeDefultStatus(true);

                var res = await SaveAsync();
                if (res)
                    return new(true, "آدرس پیش فرض شما با موفقیت تغییر کرد");
                return new(false, "خطا در تغییر آدرس پیش فرض شما لطفا مجددا تلاش فرمایید");
            }
            catch
            {

                return new(false, "خطا در تغییر آدرس پیش فرض شما لطفا مجددا تلاش فرمایید");
            }
        }
    }

}
