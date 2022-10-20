using System.ComponentModel.DataAnnotations;

using Safe_Sign.DTO.DocumentFile;

namespace Safe_Sign.DTO.Document
{
    public class UpdateDocumentDTO 
    {
        [Required]
        public ulong Id { get; set; }

        public DocumentFileDTO? File { get; set; }
    }
}
