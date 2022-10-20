using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Safe_Sign.DAL.Enumerators;

namespace Safe_Sign.DAL.Models
{
    public class Document
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public string Description { get; set; }
        
        public Guid KeyHash { get; set; }
        
        [Required]
        public DocumentStatusEnum Status { get; set; }
        
        [Required]
        public DateTime CreationDate { get; set; }
        
        [Required]
        public DateTime LastModifiedDate { get; set; }
        
        public virtual DocumentFile? File { get; set; }
        
        [Required]
        public ulong IdUser { get; set; }
        
        [ForeignKey("IdUser")]
        public virtual User? User { get; set; }
        
        public virtual ICollection<Signature>? Signatures { get; set; }

        public virtual ICollection<Marker>? Markers { get; set; }
    }
}
