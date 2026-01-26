using Query.Contract.UI.UserPanel;
using Shared.Application;
using Shared.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Ui.UserPanel
{
    internal class UserPanelQueryService : IUserPanelQueryService
    {
        private readonly IUserRepository _userRepository;

        public UserPanelQueryService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserPanelQueryModel> GetUserInfoForPanel(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return new UserPanelQueryModel()
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                PhoneNumber = user.Mobile,
                RegisterDate = user.CreateDate.ToPersainDate(),
                Avatar = user.Avatar,
                Gender = user.UserGender,
                TransactionCount = 0,
                TransactionSum = 0,

            };

        }
    }
}
