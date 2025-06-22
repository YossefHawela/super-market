using System.Data;

namespace SuperMarket.Models
{
    public class UserAccountModel
    {
        public Guid ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{nameof(ID)}:{ID}, {nameof(UserName)}:{UserName}, {nameof(Email)}:{Email}";
        }
    }
}
