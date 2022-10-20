using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Safe_Sign.DAL.Enumerators;

namespace Safe_Sign.DAL.Models
{
    public class Signature
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public Guid KeyHash { get; set; }

        [Required]
        public DateTime SignatureDate { get; set; }

        [Required]
        public SignatureTypeEnum SignatureType { get; set; }

        [Required]
        public ulong IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }

        public virtual ICollection<Document>? Documents { get; set; }
    }
}
