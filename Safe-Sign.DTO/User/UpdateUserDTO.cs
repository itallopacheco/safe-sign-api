using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.User
{
    public class UpdateUserDTO
    {
        [Required]
        public ulong Id { get; set; }

        public string? Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
