using System.ComponentModel.DataAnnotations;

using Safe_Sign.DAL.Enumerators;

namespace Safe_Sign.DTO.User
{
    public class UserDTO
    {
        public ulong Id { get; set; }

        public UserTypeEnum UserType { get; set; }

        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool EmailVerified { get; set; } = false;

        public bool IsActive { get; set; } = false;

        public ulong? IdProfile { get; set; }
    }
}
