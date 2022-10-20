using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Safe_Sign.DAL.Models
{
    public class Marker
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public ulong IdTheme { get; set; }

        [ForeignKey("IdTheme")]
        public virtual Theme? Theme { get; set; }

        public virtual ICollection<Document>? Documents { get; set; }
    }
}
