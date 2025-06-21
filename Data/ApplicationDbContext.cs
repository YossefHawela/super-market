using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperMarket.DTO;

namespace SuperMarket.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SuperMarket.DTO.ProductDTO> ProductDTO { get; set; } = default!;
        public DbSet<SuperMarket.DTO.UserDTO> userAccounts { get; set; } = default!;
    }
}
