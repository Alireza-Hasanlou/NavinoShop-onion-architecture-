using Microsoft.EntityFrameworkCore;
using Shared.Application.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Query;
using Users.Domain.User.Agg.IRepository;

namespace Users.Query.Service
{
    internal class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository _userRepository;

        public UserQueryService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserQueryModel>> GetUsersByIds(List<int> Ids)
        {
            return await _userRepository.GetUsersByIds(Ids);
        }

        public async Task<EditUserByAdminDto> GetForEditByAdminAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return null;

            return new EditUserByAdminDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Mobile = user.Mobile,
                Email = user.Email,
                AvatarName = user.Avatar,
                UserGender = user.UserGender
            };
        }

        public async Task<UserHeaderQueryModel> GetUserForHeader(int id)
        {
            if (id < 1)
                return (new());
            return await _userRepository.GetUserForHeader(id);
        }

        public async Task<AdminUserPaging> GetUsersForAdminAsync(int pageId, int take, string filter)
        {

            var model = new AdminUserPaging();
            var users = _userRepository.GetAllBy(i => !i.IsDelete);
            if (!string.IsNullOrWhiteSpace(filter))
            {
                users = users.Where(u => u.FullName.Contains(filter)
                || u.Mobile.Contains(filter)
                || u.Email.Contains(filter));

            }

            model.GetData(users, pageId, take, 5);
            model.Filter = filter;
            model.Users = await users.OrderByDescending(u => u.CreateDate)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(u => new UsersForAdminDto
                {
                    FullName = u.FullName,
                    Avatar = u.Avatar,
                    Active = u.Active,
                    Email = u.Email,
                    CreateDate = u.CreateDate,
                    IsDelete = u.IsDelete,
                    Id = u.Id,
                    Mobile = u.Mobile
                }).ToListAsync();

            return model;
        }

        public Task<UserDetailDto> GetUserDetailForAdminAsync(int userId)
        {
            return _userRepository.GetUserDetailAsync(userId);
        }

        public async Task<AdminUserPaging> GetDeletedUserForAdmin(int pageId, int take, string filter)
        {
            var model = new AdminUserPaging();
            var users = _userRepository.GetAllBy(i=>i.IsDelete);
            if (!string.IsNullOrWhiteSpace(filter))
            {
                users = users.Where(u => u.FullName.Contains(filter)
                || u.Mobile.Contains(filter)
                || u.Email.Contains(filter));

            }

            model.GetData(users, pageId, take, 5);
            model.Filter = filter;
            model.Users = await users.OrderByDescending(u => u.CreateDate)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(u => new UsersForAdminDto
                {
                    FullName = u.FullName,
                    Avatar = u.Avatar,
                    Active = u.Active,
                    Email = u.Email,
                    CreateDate = u.CreateDate,
                    IsDelete = u.IsDelete,
                    Id = u.Id,
                    Mobile = u.Mobile
                }).ToListAsync();

            return model;
        }
    }
}
