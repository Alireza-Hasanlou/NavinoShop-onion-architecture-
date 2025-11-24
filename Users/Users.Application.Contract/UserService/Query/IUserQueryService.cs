using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contract.UserService.Query
{
    public interface IUserQueryService
    {
        Task<EditUserByUserDto> GetForEditByUserAsync(int userId);
        Task<EditUserByAdminDto> GetForEditByAdminAsync(int userId);
        Task<List<UserQueryModel>> GetUsersByIds(List<int> Ids);
        
    }

    public class UserQueryModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
    }
}
