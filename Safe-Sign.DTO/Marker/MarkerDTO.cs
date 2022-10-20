using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.Marker
{
    public class MarkerDTO
    {
        public ulong Id { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public ulong IdTheme { get; set; }
    }
}
