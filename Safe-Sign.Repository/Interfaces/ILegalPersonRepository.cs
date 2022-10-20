using Safe_Sign.DTO.LegalPerson;

namespace Safe_Sign.Repository.Interfaces
{
    public interface ILegalPersonRepository
    {
        LegalPersonDTO GetLegalPersonById(ulong idPerson);

        IList<LegalPersonDTO> GetAllLegalPersons();

        LegalPersonDTO CreateLegalPerson(LegalPersonDTO person);

        LegalPersonDTO UpdateLegalPerson(UpdateLegalPersonDTO person);

        void DeleteLegalPerson(ulong idPerson);
    }
}
