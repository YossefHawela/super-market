using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Models
{
    public class AdminSettingsModel
    {
        [Required]
        public string SiteName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string ContactEmail { get; set; } = string.Empty;

        public bool MaintenanceMode { get; set; }
    }
}
