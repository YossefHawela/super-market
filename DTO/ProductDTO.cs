using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SuperMarket.DTO
{
    public class ProductDTO
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
    }
}
