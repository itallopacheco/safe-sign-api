using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.SGP
{
    public class SGPSignInDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
