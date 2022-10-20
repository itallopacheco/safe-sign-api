using Safe_Sign.DTO.Document;

namespace Safe_Sign.Repository.Interfaces
{
    public interface IDocumentRepository
    {
        IList<DocumentDTO> GetAll();
        
        DocumentDTO CreateDocument (DocumentDTO document, string virtualPath);
        
        DocumentDTO UpdateDocument(UpdateDocumentDTO updateDocumentDTO);

        bool GetDocumentByToken (string token);
        
        void SignDocument (ulong IdDocument);
    }
}
