using Safe_Sign.DAL.DAO;
using Safe_Sign.DAL.Models;
using Safe_Sign.DAL.DAO.Base;
using Safe_Sign.DTO.Signature;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.Repository
{
    public class SignatureRepository : ISignatureRepository
    {
        private readonly DocumentDAO _documentDAO;
        private readonly SignatureDAO _signatureDAO;
        private readonly UserDAO _userDAO;

        public SignatureRepository(SignatureDAO signatureDAO, DocumentDAO documentDAO, UserDAO userDAO)
        {
            _signatureDAO = signatureDAO;
            _documentDAO = documentDAO;
            _userDAO = userDAO;
        }

        public SignatureDTO GetById(ulong id)
        {
            Signature? signature = _signatureDAO.GetById(id);

            if (signature is null) throw new NullReferenceException("");

            Document? documentReference = _documentDAO.GetAll().FirstOrDefault(d => d.Signatures is not null && d.Signatures.Any(s => s.Id.Equals(id)));

            if (documentReference is null) throw new NullReferenceException("This signature was not found");

            SignatureDTO finalDTO = new()
            {
                Id = signature.Id,
                KeyHash = signature.KeyHash,
                SignatureDate = signature.SignatureDate,
                SignatureType = signature.SignatureType,
                SignatureTypeDescription = signature.SignatureType.ToString(),
                IdUser = signature.IdUser,
                IdDocument = documentReference.Id
            };

            return finalDTO;
        }

        public SignatureDTO GetSignatureByToken(string token)
        {
            Signature signature = _signatureDAO.GetSignatureByToken(token);

            if (signature is null) throw new NullReferenceException("This signature was not found");

            Document? documentReference = _documentDAO.GetAll().FirstOrDefault(d => d.Signatures is not null && d.Signatures.Any(s => s.KeyHash.Equals(token)));

            if (documentReference is null) throw new NullReferenceException("This signature was not found");

            SignatureDTO signatureDTO = new()
            {
                Id = signature.Id,
                KeyHash = signature.KeyHash,
                SignatureDate = signature.SignatureDate,
                SignatureType = signature.SignatureType,
                SignatureTypeDescription = signature.SignatureType.ToString(),
                IdUser = signature.IdUser,
                IdDocument = documentReference.Id
            };

            return signatureDTO;
        }

        public IList<SignatureDTO> GetAll()
        {
            IEnumerable<Signature> signatures = _signatureDAO.GetAll();

            if (signatures is null) throw new NullReferenceException("Error on search all signatures");

            List<SignatureDTO> signaturesDTO = new();

            foreach (Signature s in signatures)
            {
                Signature? currentSignature = _signatureDAO.GetById(s.Id);

                if (currentSignature is null) throw new NullReferenceException($"Error on search for signature information of signature of id {s.Id}");

                Document? documentReference = _documentDAO.GetAll().FirstOrDefault(d => d.Signatures is not null && d.Signatures.Any(s => s.KeyHash.Equals(currentSignature.KeyHash)));

                if (documentReference is null) throw new NullReferenceException("This signature was not found");

                SignatureDTO signatureDTO = new()
                {
                    Id = currentSignature.Id,
                    KeyHash = currentSignature.KeyHash,
                    SignatureDate = currentSignature.SignatureDate,
                    IdUser = currentSignature.IdUser,
                    IdDocument = documentReference.Id,
                    SignatureType = currentSignature.SignatureType,
                    SignatureTypeDescription = currentSignature.SignatureType.ToString()
                };

                signaturesDTO.Add(signatureDTO);
            }

            return signaturesDTO;
        }

        public SignatureDTO CreateSignature(SignatureDTO signature)
        {
            Document document = _documentDAO.GetDocumentWithDependeciesById(signature.IdDocument); //PUXANDO SIGNATURES = NULL
            ICollection<Signature> signatures = new List<Signature>();

            if (document is null) throw new NullReferenceException("The target document cannot be found.");

            Signature newSignature = new()
            {
                KeyHash = Guid.NewGuid(),
                SignatureDate = DateTime.Now,
                SignatureType = signature.SignatureType,
                IdUser = signature.IdUser,
            };
            
            Signature _signature = _signatureDAO.Create(newSignature);

            document.Signatures.Add(_signature);
            
            _documentDAO.Update(document);

            SignatureDTO finalDTO = new()
            {
                Id = _signature.Id,
                KeyHash = _signature.KeyHash,
                SignatureDate = _signature.SignatureDate,
                SignatureType = _signature.SignatureType,
                SignatureTypeDescription = _signature.SignatureType.ToString(),
                IdUser = _signature.IdUser,
                IdDocument = signature.IdDocument
            };
                                   
            return finalDTO;
        }

        public SignatureDTO UpdateSignature(SignatureDTO signature)
        {
            throw new NotImplementedException();
        }

        public void DeleteSignature(ulong id)
        {
            Signature? signature = _signatureDAO.GetById(id);

            if (signature is null) throw new NullReferenceException("The signature was not found");

            else _signatureDAO.Delete(signature.Id);
        }
    }
}
