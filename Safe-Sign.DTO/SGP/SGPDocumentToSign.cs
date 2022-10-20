using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace Safe_Sign.DTO.SGP
{
    public class SGPDocumentToSign
    {
        [Required]
        public IFormFile SgpFile { get; set; }

        [Required]
        public string UserLogin { get; set; }
    }
}
