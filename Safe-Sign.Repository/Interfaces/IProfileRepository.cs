using Safe_Sign.DTO.Person;
using Safe_Sign.DTO.Profile;
using Safe_Sign.DTO.LegalPerson;

namespace Safe_Sign.Repository.Interfaces
{
    public interface IProfileRepository
    {
        ProfileDTO GetProfileById(ulong idProfile);

        IList<ProfileDTO> GetAllProfiles();

        IList<PersonDTO> GetAllPersonsAssignedToProfile(ulong idProfile);

        IList<LegalPersonDTO> GetAllLegalPersonsAssignedToProfile(ulong idProfile);

        ProfileDTO CreateProfile(ProfileDTO profile);

        void AssignPersonToProfile(ulong idPerson, ulong idProfile);
            
        void AssignLegalPersonToProfile(ulong idLegalPerson, ulong idProfile);

        void SwitchProfileActiveStatus(ulong idProfile);

        void DeleteProfile(ulong idProfile);
    }
}
