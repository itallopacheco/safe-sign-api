using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Safe_Sign.DTO.DocumentFile
{
    public class DocumentFileDTO
    {
        public ulong? Id { get; set; }

        [Required]
        [MinLength(5)]
        public string FileName { get; set; }

        [Required]
        public IFormFile FormFile { get; set; }

        public string? FilePath { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        
        public ulong? IdDocument { get; set; }
    }
}
