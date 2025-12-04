using Emails.Domailn.MessageUserAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.Admin.Email.EmailUser;
using Query.Contract.Admin.Email.MessageUser;
using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Admin.Email.MessageUser
{
    internal class MessageUserAdminQuery : IMessageUserAdminQuery
    {
        private readonly IMessageUserRepository _messageUserRepository;
        private readonly IUserRepository _userRepository;

        public MessageUserAdminQuery(IMessageUserRepository messageUserRepository, IUserRepository userRepository)
        {
            _messageUserRepository = messageUserRepository;
            _userRepository = userRepository;
        }

        public async Task<MessageUserAdminPaging> GetMessagesForAdmin(MessageStatus status, int pageId, int take, string Filter = "")
        {

            var Messages = _messageUserRepository.GetAllBy();

            if (status != MessageStatus.همه)
                Messages = Messages.Where(s => s.Status == status);

            if (!string.IsNullOrWhiteSpace(Filter))
                Messages = Messages.Where(e =>
                      e.Email.ToLower().Contains(Filter.ToLower()) ||
                      e.Subject.ToLower().Contains(Filter.ToLower()) ||
                      e.PhoneNumber.ToLower().Contains(Filter.ToLower()) ||
                      e.FullName.ToLower().Contains(Filter.ToLower()) ||
                      e.Message.ToLower().Contains(Filter.ToLower()));


            MessageUserAdminPaging model = new();
            model.GetData(Messages, pageId, take, 5);
            model.Filter = Filter;
            model.Status = status;
            model.Messages = await Messages.Skip(model.Skip).Take(take).Select(m => new MessageUserAdminQueryModel
            {
                Id = m.Id,
                Subject = m.Subject,
                Email = m.Email,
                FullName = m.FullName,
                PhoneNumber = m.PhoneNumber,
                CreateDate = m.CreateDate.ToPersainDate(),
                UserId = m.UserId,
                Status = m.Status,
            }).OrderByDescending(i => i.Id).ToListAsync();

            foreach (var item in model.Messages)
            {
                var user = await _userRepository.GetByIdAsync(item.Id);
                item.UserName = !string.IsNullOrEmpty(user.FullName) ? user.FullName : user.Mobile;
            }

            return model;

        }
        public async Task<MessageUserDetailAdminQueryModel> GetMessageDetailForAdmin(int id)
        {
            var m = await _messageUserRepository.GetByIdAsync(id);
            MessageUserDetailAdminQueryModel model = new()
            {
                AnswerEmail = m.AnswerEmail,
                AnswerSms = m.AnswerSms,
                CreationDate = m.CreateDate.ToPersainDate(),
                Email = m.Email,
                FullName = m.FullName,
                Id = id,
                Message = m.Message,
                PhoneNumber = m.PhoneNumber,
                Status = m.Status,
                Subject = m.Subject,
                UseName = "",
                UserId = m.UserId
            };
            if (model.UserId > 0)
            {
                var user = await _userRepository.GetByIdAsync(id);
                model.UseName = string.IsNullOrEmpty(user.FullName) ? user.Mobile : user.FullName;
            }
            return model;
        }
    }
}
