using Microsoft.EntityFrameworkCore;

using Safe_Sign.DAL.Models;
using Safe_Sign.DAL.DAO.Base;

namespace Safe_Sign.DAL.DAO
{
    public class SignatureDAO : BaseDAO<Signature>
    {

        public Signature GetSignatureByToken(string token)
        {
            Signature? signature = _context.Set<Signature>().FirstOrDefault(s => s.KeyHash.Equals(token));

            if (signature is null) throw new NullReferenceException();

            else return signature;
        }
        
        public ICollection<Signature> GetSignaturesByDocument(ulong id)
        {
            ICollection<Signature> Signatues = new List<Signature>();
            ICollection<Document> Documents = new List<Document>();

            Documents = _context.Set<Document>().Include(s => s.Signatures).ToList();

            return Signatues;
        }
    }
}
