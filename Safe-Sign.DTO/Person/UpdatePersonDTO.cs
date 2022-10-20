using System.ComponentModel.DataAnnotations;

using Safe_Sign.DTO.User;

namespace Safe_Sign.DTO.Person
{
    public class UpdatePersonDTO
    {
        public ulong Id { get; set; }

        public UpdateUserDTO? User { get; set; }

        [RegularExpression(@"\([0-9]{2}\)\s[9]{1}[0-9]{4}-[0-9]{4}")]
        public string? PrimaryPhone { get; set; }

        [RegularExpression(@"\([0-9]{2}\)\s[9]{1}[0-9]{4}-[0-9]{4}")]
        public string? SecondaryPhone { get; set; }
    }
}
