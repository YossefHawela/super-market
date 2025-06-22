using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;
using SuperMarket.Data;
using SuperMarket.DTO;
using System.Security.Claims;

namespace SuperMarket.Data
{
    public class DataConnector
    {

        private readonly ApplicationDbContext _context;

        public IEnumerable<UserDTO> Users => _context.userAccounts.ToList();
        public IEnumerable<ProductDTO> Products => _context.ProductDTO.ToList();

        public IEnumerable<LogDTO> AdminLogs => _context.AdminLogs.ToList();

        public DataConnector(ApplicationDbContext context)
        {
            _context = context;
        }


 
        public void Add<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "ToDo item cannot be null.");
            }

            _context.Add(item);

            _context.SaveChanges();
        }

        public bool IsUserNameAvailable(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("User name cannot be null or empty.", nameof(userName));
            }
            return !_context.userAccounts.Any(u => u.UserName.ToLower() == userName.ToLower());
        }

        public bool IsEmailAvailable(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }
            return !_context.userAccounts.Any(u => u.Email.ToLower() == email.ToLower());
        }

        public UserDTO? GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }
            return _context.userAccounts.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        public UserDTO? GetUserByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("User name cannot be null or empty.", nameof(userName));
            }
            return _context.userAccounts.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
        }


        public UserDTO? GetUserByID(Guid Id)
        {
            if (Id == default)
            {
                throw new ArgumentException("ID cannot be null.", nameof(Id));
            }
            return _context.userAccounts.FirstOrDefault(u => u.Id == Id);
        }
        public void Update<T>(T Item)
            where T : class
        {
            _context.Update<T>(Item);

            _context.SaveChanges();
        }


        public void Delete<T>(T Item)
            where T : class
        {
            _context.Remove<T>(Item);

            _context.SaveChanges();
        }

    }
}
