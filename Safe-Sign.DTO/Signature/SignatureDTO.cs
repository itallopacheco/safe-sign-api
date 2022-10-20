using System.ComponentModel.DataAnnotations;

using Safe_Sign.DAL.Enumerators;

namespace Safe_Sign.DTO.Signature
{
    public class SignatureDTO
    {
        public ulong Id { get; set; }

        [Required]
        public Guid KeyHash { get; set; }

        [Required]
        public DateTime SignatureDate { get; set; }

        [Required]
        public ulong IdUser { get; set; }

        [Required]
        public ulong IdDocument { get; set; }

        public SignatureTypeEnum SignatureType { get; set; }

        public string? SignatureTypeDescription { get; set; }
    }
}
