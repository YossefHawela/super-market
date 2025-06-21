using BCrypt.Net;
using SuperMarket.DTO;
using System.Collections.ObjectModel;
using System.Security.Claims;
using SuperMarket.Models;


namespace SuperMarket.Mapper
{
    public static class ExtensionMapper
    {
       

        public static UserDTO ToUserDTO(this RegisterAccountModel model,bool isPasswordHashed = true)
        {

            // hash the password before saving it to the database

            DateTime creationTime = DateTime.UtcNow;

            var hashedPassword = model.Password; // Default to the plain password if hashing is not required

            if (isPasswordHashed)
            {
                 hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password); // Generates salt + hashes

            }




            return new UserDTO
            {
                UserName = model.UserName,
                Password = hashedPassword,
                Email = model.Email,
                CreatedAt = creationTime,
                Role = model.Role
            };
        }


        public static UserDTO ToSecureUserDTO(this UserDTO model, bool isPasswordHashed = true)
        {

            // hash the password before saving it to the database

            DateTime creationTime = DateTime.UtcNow;

            var hashedPassword = model.Password; // Default to the plain password if hashing is not required

            if (isPasswordHashed)
            {
                hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password); // Generates salt + hashes

            }




            return new UserDTO
            {
                UserName = model.UserName,
                Password = hashedPassword,
                Email = model.Email,
                CreatedAt = creationTime,
                Role = model.Role
            };
        }
        public static LoginAccountModel ValidatePassword(this LoginAccountModel LoginModel, UserDTO userAccount,bool isPasswordHashed = true)
        {

            bool isValid = LoginModel.Password == userAccount.Password;

            if (userAccount != null && !string.IsNullOrEmpty(userAccount.Password) && !string.IsNullOrEmpty(LoginModel.Password)&&isPasswordHashed)
            {

                isValid = BCrypt.Net.BCrypt.Verify(LoginModel.Password,userAccount.Password);
            }


            return new LoginAccountModel
            {
                UserNameOrEmail = LoginModel.UserNameOrEmail,
                Password = LoginModel.Password,
                isValid = isValid,
                Role = userAccount!.Role
            };
        }
        public static UserAccountModel ToUserModel(this UserDTO dto)
        {
            return new UserAccountModel
            {
                ID = dto.Id,
                UserName = dto.UserName,
                Email = dto.Email,
            };
        }

        public static ProductModel ToProductModel(this ProductDTO productDTO)
        {
            return new ProductModel
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Price = productDTO.Price,

            };
        }
        public static IEnumerable<ProductModel> ToProductModels(this IEnumerable<ProductDTO> productDTOs)
        {
            return productDTOs.Select(p =>
            {
                return p.ToProductModel();

            });
        }

        public static ProductDTO ToProductDTO(this ProductModel productModel)
        {
            return new ProductDTO
            {
                Id = productModel.Id,
                Name = productModel.Name,
                Price = productModel.Price,

            };
        }

        public static IEnumerable<ProductDTO> ToProductDTOes(this IEnumerable<ProductModel> productModels)
        {
            return productModels.Select(p => p.ToProductDTO());
            
        }
    }
}
