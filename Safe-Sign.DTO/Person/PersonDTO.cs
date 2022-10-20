using System.ComponentModel.DataAnnotations;

using Safe_Sign.DTO.User;

namespace Safe_Sign.DTO.Person
{
    public class PersonDTO
    {
        public ulong Id { get; set; }

        public UserDTO User { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [StringLength(11)]
        public string CPF { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string MotherName { get; set; }

        [RegularExpression(@"\([0-9]{2}\)\s[9]{1}[0-9]{4}-[0-9]{4}")]
        public string PrimaryPhone { get; set; }

        [RegularExpression(@"\([0-9]{2}\)\s[9]{1}[0-9]{4}-[0-9]{4}")]
        public string? SecondaryPhone { get; set; }
    };
}
