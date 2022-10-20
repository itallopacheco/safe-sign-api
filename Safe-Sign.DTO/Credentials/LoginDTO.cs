using System.ComponentModel.DataAnnotations;

using Safe_Sign.DAL.Enumerators;

namespace Safe_Sign.DTO.Credentials
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Range(0, 1)]
        public UserTypeEnum userType { get; set; }
    }
}
