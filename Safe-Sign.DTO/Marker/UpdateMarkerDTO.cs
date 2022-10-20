using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.Marker
{
    public class UpdateMarkerDTO : MarkerDTO
    {
        [Required]
        public new ulong Id { get; set; }
    }
}
