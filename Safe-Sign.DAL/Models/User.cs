using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Safe_Sign.DAL.Enumerators;

namespace Safe_Sign.DAL.Models
{
    public class User
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public UserTypeEnum UserType { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool EmailVerified { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = false;

        public ulong? IdProfile { get; set; }

        [ForeignKey("IdProfile")]
        public virtual Profile? Profile { get; set; }

        public virtual Person? Person { get; set; }

        public virtual LegalPerson? LegalPerson { get; set; }

        public virtual ICollection<Document>? Documents { get; set; }

        public virtual ICollection<Signature>? Signatures { get; set; }

    }
}