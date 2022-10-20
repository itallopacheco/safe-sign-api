using Safe_Sign.DTO.Person;
using Safe_Sign.DTO.Credentials;

namespace Safe_Sign.Repository.Interfaces
{
    public interface IPersonRepository
    {
        PersonDTO GetPersonById(ulong idPerson);

        PersonDTO GetPersonByName(string name);

        PersonDTO GetPersonByCPF(string cpf);

        PersonDTO GetPersonByEmail(string cpf);

        CredentialsDTO GetPersonByCredentials(string username, string password);

        IList<PersonDTO> GetAllPersons();

        PersonDTO CreatePerson(PersonDTO person);

        PersonDTO UpdatePerson(UpdatePersonDTO person);

        string SendEmail(ulong idPerson);

        void SwitchUserEmailStatus(string toDecode);

        bool SwitchUserActiveStatus(ulong idPerson);

        void DeletePerson(ulong idPerson);
    }
}
