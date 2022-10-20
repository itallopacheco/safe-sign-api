using System.ComponentModel.DataAnnotations;

using Safe_Sign.DAL.Enumerators;

namespace Safe_Sign.DTO.Profile
{
    public class ProfileDTO
    {
        public ulong Id { get; set; }

        [Required]
        [Range(0, 1)]
        public ProfileEnum Type { get; set; }

        public string? TypeDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
