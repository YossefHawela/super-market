using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarket.DTO
{
    [Table("AdminLogs")]
    public class LogDTO
    {
        [Key]
        public Guid ID { get; set; }

        public string UserName { get; set; } = string.Empty;

        public DateTime dateCreated { get; set; } = DateTime.UtcNow;
        public string logInforamtion { get; set; } = string.Empty;
    }
}
