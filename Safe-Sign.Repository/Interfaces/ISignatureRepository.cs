using Safe_Sign.DTO.Signature;

namespace Safe_Sign.Repository.Interfaces
{
    public interface ISignatureRepository
    {
        SignatureDTO GetById(ulong id);
        
        SignatureDTO GetSignatureByToken(string token);

        IList<SignatureDTO> GetAll();

        SignatureDTO CreateSignature(SignatureDTO signature);

        SignatureDTO UpdateSignature(SignatureDTO signature);

        void DeleteSignature(ulong id);
    }
}
