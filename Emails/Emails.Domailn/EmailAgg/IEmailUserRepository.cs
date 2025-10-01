using Utility.Shared.Domain;

namespace Emails.Domailn.EmailAgg
{
    public interface IEmailUserRepository : IGenericRepository<EmailUser, int>
    {
        Task< bool> CreateList(List<EmailUser> emailUsers);
       Task< EmailUser > GetByEmail(string email);
    }
}
