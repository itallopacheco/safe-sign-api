using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DAL.Models
{
    public class Theme
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual Marker? Marker { get; set; }
    }
}
