using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.Theme
{
    public class UpdateThemeDTO : ThemeDTO
    {
        [Required]
        public new ulong Id { get; set; }
    }
}
