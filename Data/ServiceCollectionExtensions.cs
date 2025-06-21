using SuperMarket.DTO;

namespace SuperMarket.Data
{
    public static class ServiceCollectionExtensions
    {


        public static IServiceCollection FeedDb(this IServiceCollection services,ApplicationDbContext context) 
        {
            if (!context.userAccounts.Any(uc=>uc.Role == "Admin"))
            {
                var adminEmail = "admin";
                var adminPassword = "admin";

                if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
                    throw new Exception("Missing admin credentials in environment.");

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(adminPassword);

                context.Add(new UserDTO
                {
                    UserName = "admin",
                    Email = adminEmail,
                    Password = hashedPassword,
                    Role = "Admin"
                });

                context.SaveChanges();

            }
            return services;

        }

    }
}
