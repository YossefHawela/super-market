using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarket.DTO;
[Table("Users")]
public class UserDTO
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public string Role { get; set; } = "User"; // Default role


    public override string ToString()
    {
        return $"{nameof(Id)}:{Id}, {nameof(UserName)}:{UserName}, {nameof(Email)}:{Email}, {nameof(Role)}:{Role}";
    }
}
