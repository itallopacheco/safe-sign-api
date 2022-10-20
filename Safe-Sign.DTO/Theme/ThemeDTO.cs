using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.Theme
{
    public class ThemeDTO
    {
        public ulong Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}
