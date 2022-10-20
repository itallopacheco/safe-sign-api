using Microsoft.EntityFrameworkCore;

using Safe_Sign.DAL.Models;
using Safe_Sign.DAL.DAO.Base;

namespace Safe_Sign.DAL.DAO
{
    public class DocumentDAO : BaseDAO<Document>
    {
        public Document GetDocumentWithDependeciesById(ulong id)
        {
            Document document = _context.Set<Document>()
                .Where(d => d.Id == id)
                .Include(d => d.Signatures)
                .Include(d => d.File)
                .FirstOrDefault();

            if (document is null) throw new NullReferenceException();
            else
            {
                return document;
            }
            
        }
        
        public Document GetDocumentWithDependeciesByHash(string token)
        {
            Document document = _context.Set<Document>()
                .Include(d => d.Signatures)
                .Include(d => d.File)
                .Include(d => d.Markers)
                .FirstOrDefault(d => d.KeyHash.ToString() == token);
            if(document is null) throw new NullReferenceException();
            else
            {
                return document;
            }
        }
    }
}
