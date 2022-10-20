using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Safe_Sign.DAL.Models
{
    public class DocumentFile
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public string FileName { get; set; }
        
        [Required]
        public string FilePath { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [Required]
        public ulong IdDocument { get; set; }

        [ForeignKey("IdDocument")]
        public virtual Document? Document { get; set; }
    }
}
