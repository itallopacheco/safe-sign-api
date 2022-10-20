using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.SGP
{
    public class SGPAuthDTO
    {
        [Required]
        public string Access_token { get; set; }

        [Required]
        public string Token_type { get; set; }

        [Required]
        public string Expires_in { get; set; }

        [Required]
        public string Refresh_token { get; set; }

        [Required]
        public string Scope { get; set; }
    }
}
