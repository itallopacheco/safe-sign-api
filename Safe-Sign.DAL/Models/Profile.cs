using System.ComponentModel.DataAnnotations;

using Safe_Sign.DAL.Enumerators;

namespace Safe_Sign.DAL.Models
{
    public class Profile
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public ProfileEnum Type { get; set; }

        [Required]
        public bool IsActive { get; set; } = false;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
