using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.Credentials
{
    public class CredentialsDTO
    {
        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
