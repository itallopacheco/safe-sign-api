using Microsoft.AspNetCore.Http;

using Safe_Sign.DTO.SGP;
using Safe_Sign.DAL.Models;

namespace Safe_Sign.Repository.Interfaces
{
    public interface ISGPRepository
    {
        void DeleteTempDirectory(string directoryPath);

        void DeleteTempFile(string filePath);
        
        string InsertSignature(string fileDirectoryPath, string filePath, int signaturesNumber, Document document, IList<Signature> signatures, IList<Person> persons);

        SGPSignedDocumentDTO PrepareDocumentToExport(string signedFilePath, bool signed = false);
        
        (string, string) SaveFileAsTemp(IFormFile file);
        
        SGPSignedDocumentDTO SignSGPDocument(IFormFile sgpFile, string userLogin);
    }
}
