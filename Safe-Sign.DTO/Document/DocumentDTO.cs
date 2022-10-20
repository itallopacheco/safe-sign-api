using System.ComponentModel.DataAnnotations;

using Safe_Sign.DTO.Marker;
using Safe_Sign.DTO.Signature;
using Safe_Sign.DAL.Enumerators;
using Safe_Sign.DTO.DocumentFile;

namespace Safe_Sign.DTO.Document
{
    public class DocumentDTO
    {
        public ulong? Id { get; set; }

        public Guid? KeyHash { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        [Range(0, 3)]
        public DocumentStatusEnum Status { get; set; }

        public string? StatusDescription { get; set; }

        [Required]
        public ulong IdUser { get; set; }
        
        [Required]
        public DocumentFileDTO File { get; set; }
        
        public DateTime? CreationDate { get; set; }
        
        public DateTime? LastModifiedDate { get; set; }

        public IList<SignatureDTO>? Signatures { get; set; }

        public IList<MarkerDTO>? Markers { get; set; }

    }
}
