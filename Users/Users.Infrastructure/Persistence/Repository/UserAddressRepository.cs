using Microsoft.EntityFrameworkCore;
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

        public async Task<int> GetUserAddressCount()
        {
            return await _context.UserAddresses.CountAsync();
        }
    }

}
