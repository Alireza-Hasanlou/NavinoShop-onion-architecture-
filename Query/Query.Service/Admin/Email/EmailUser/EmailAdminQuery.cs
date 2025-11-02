using Emails.Domailn.EmailAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.Admin.Email.EmailUser;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Admin.Email.EmailUser
{
    internal class EmailAdminQuery : IEmailAdminQuery
    {
        private readonly IEmailUserRepository _emailRepository;
        private readonly IUserRepository _userRepository;

        public EmailAdminQuery(IEmailUserRepository emailRepository, IUserRepository userRepository)
        {
            _emailRepository = emailRepository;
            _userRepository = userRepository;
        }

        public async Task<EmailUserAdminPaging> GetAllEmailForAdmin(int pageId, int take, string? filter = "")
        {
            var Emails = _emailRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(filter))
                Emails = _emailRepository.GetAllBy(e => e.Email.ToLower().Contains(filter.ToLower()));


            EmailUserAdminPaging model = new();
            model.GetData(Emails, pageId, take, 5);
            model.Filter = filter;
            model.Emails = await Emails.Skip(model.Skip).Take(take).Select(x => new EmailAdminQueryModel
            {
                Email = x.Email,
                Id = x.Id,
                UserId = x.UserId,
                Active=x.Active

            }).OrderByDescending(i => i.Id).ToListAsync();

            model.Emails.ForEach(async x =>
            {
                var user = await _userRepository.GetByIdAsync(x.UserId);
                x.UserName = !string.IsNullOrEmpty(user.FullName) ? user.FullName : user.Mobile;
            });

            return model;



        }
    }
}
