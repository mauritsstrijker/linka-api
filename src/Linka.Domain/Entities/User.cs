using Linka.Domain.Common;
using Linka.Domain.Enums;
using Linka.Domain.Helpers;

namespace Linka.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
        public static User Create
           (
           string username,
           string email,
           string password,
           UserType type
           )
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = username.Trim(),
                Email = email.Trim(),
                Password = PasswordHelper.HashPassword(password),
                Type = type
            };
        }
    }
}
