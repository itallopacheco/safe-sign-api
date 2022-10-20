using Safe_Sign.DTO.User;
using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.LegalPerson
{
    public class UpdateLegalPersonDTO
    {
        [Required]
        public ulong Id { get; set; }

        public UpdateUserDTO? User { get; set; }

        [MinLength(3)]
        [MaxLength(50)]
        public string? CompanyName { get; set; }

        [StringLength(14)]
        public string? CNPJ { get; set; }

        [MinLength(3)]
        [MaxLength(50)]
        public string? LegalAgent { get; set; }

        [RegularExpression(@"\([0-9]{2}\)\s[9]{1}[0-9]{4}-[0-9]{4}")]
        public string? PrimaryPhone { get; set; }

        [RegularExpression(@"\([0-9]{2}\)\s[9]{1}[0-9]{4}-[0-9]{4}")]
        public string? SecondaryPhone { get; set; }
    }
}
