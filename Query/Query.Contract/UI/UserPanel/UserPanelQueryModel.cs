using Shared.Domain.Enums;
using Shared.Ui;

namespace Query.Contract.UI.UserPanel
{
    public class UserPanelQueryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string RegisterDate { get; set; }
        public int TransactionCount { get; set; }
        public int TransactionSum { get; set; }
        public string Avatar { get; set; }
    }
}
