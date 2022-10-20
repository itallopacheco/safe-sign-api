using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Safe_Sign.DAL.Models
{
    public class Person
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string PrimaryPhone { get; set; }

        public string? SecondaryPhone { get; set; }

        [Required]
        public string MotherName { get; set; }

        [Required]
        public ulong IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }
    }
}