using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Safe_Sign.DAL.Models
{
    public class LegalPerson
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CNPJ { get; set; }

        [Required]
        public string LegalAgent { get; set; }

        [Required]
        public string PrimaryPhone { get; set; }

        public string SecondaryPhone { get; set; }

        [Required]
        public ulong IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }
    }
}
