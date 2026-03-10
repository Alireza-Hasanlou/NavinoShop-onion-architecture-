using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Query;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using Users.Infrastructure.Persistence.Context;

namespace Users.Infrastructure.Persistence.Repository
{
    internal class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserQueryModel>> GetUsersByIds(List<int> Ids)
        {
            return await _context.Users.Where(i => Ids.Contains(i.Id)).Select(u => new UserQueryModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Mobile = u.Mobile,
            }).ToListAsync();
        }

        public async Task<User> GetByMobile(string mobile)
        {
            return await _context.Users.SingleOrDefaultAsync(m => m.Mobile == mobile);
        }

        public async Task<EditUserByUserDto> GetForEditByUserAsync(int userId)
        {
            return await _context.Users.Where(i => i.Id == userId)
                .Select(u => new EditUserByUserDto
                {
                    FullName = u.FullName,
                    Email = u.Email,
                    Mobile = u.Mobile,
                    UserGender = u.UserGender,
                    AvatarName = u.Avatar

                }).SingleAsync();
        }

        public async Task<UserHeaderQueryModel> GetUserForHeader(int id)
        {
            return await _context.Users.Where(u => u.Id == id)
                .Select(u => new UserHeaderQueryModel
                {
                    FullName = string.IsNullOrEmpty(u.FullName) ? u.Mobile : u.FullName,
                    Avatar = u.Avatar,
                    Mobile = u.Mobile
                }).SingleAsync();
        }



        public async Task<UserDetailDto> GetUserDetailAsync(int userId)
        {
            var query = await _context.Users.Where(i => i.Id == userId)
                .Include(u => u.UserRoles)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync();

            var userDetail = new UserDetailDto
            {
                FullName = query.FullName,
                Avatar = query.Avatar,
                Mobile = query.Mobile,
                Active = query.Active,
                CreateDate = query.CreateDate,
                Email = query.Email,
                Id = userId,
                IsDelete = query.IsDelete,
                UserRole = new List<string>()
            };
            foreach (var item in query.UserRoles)
            {
                userDetail.UserRole.Add(item.Role.Title);
            }
            return userDetail;
        }

        public async Task<User> GetForEditByAdmin(int userId)
        {
            return await _context.Users.Where(i => i.Id == userId).Include(u => u.UserRoles).SingleAsync();
        }
    }
}