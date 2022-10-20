using System.ComponentModel.DataAnnotations;

using Safe_Sign.DTO.User;

namespace Safe_Sign.DTO.LegalPerson
{
    public class LegalPersonDTO
    {
        public ulong Id { get; set; }

        public UserDTO User { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(14)]
        public string CNPJ { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LegalAgent { get; set; }

        [RegularExpression(@"\([0-9]{2}\)\s[9]{1}[0-9]{4}-[0-9]{4}")]
        public string PrimaryPhone { get; set; }

        [RegularExpression(@"\([0-9]{2}\)\s[9]{1}[0-9]{4}-[0-9]{4}")]
        public string SecondaryPhone { get; set; }
    }
}
