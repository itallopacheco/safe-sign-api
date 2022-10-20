using Safe_Sign.DAL.DAO;
using Safe_Sign.DTO.Person;
using Safe_Sign.DAL.Models;
using Safe_Sign.DTO.Profile;
using Safe_Sign.DAL.DAO.Base;
using Safe_Sign.DTO.LegalPerson;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ProfileDAO _profileDAO;
        private readonly IDAO<LegalPerson> _legalPersonDAO;
        private readonly IDAO<Person> _personDAO;
        private readonly UserDAO _userDAO;

        public ProfileRepository(IDAO<LegalPerson> legalPersonDAO, ProfileDAO profileDAO, IDAO<Person> personDAO, UserDAO userDAO)
        {
            _legalPersonDAO = legalPersonDAO;
            _profileDAO = profileDAO;
            _personDAO = personDAO;
            _userDAO = userDAO;
        }

        public ProfileDTO GetProfileById(ulong idProfile)
        {
            Profile? profile = _profileDAO.GetById(idProfile);

            if (profile is null) throw new NullReferenceException("The profile was not found");

            else
            {
                ProfileDTO newDTO = new()
                {
                    Id = profile.Id,
                    Type = profile.Type,
                    TypeDescription = profile.Type.ToString(),
                    IsActive = profile.IsActive,
                };

                return newDTO;
            }
        }

        public IList<ProfileDTO> GetAllProfiles()
        {
            IEnumerable<Profile> profiles = _profileDAO.GetAll();

            if (profiles is null) throw new NullReferenceException("Error on get all profiles");

            else
            {
                List<ProfileDTO> profilesDTO = new();

                foreach (Profile profile in profiles)
                {
                    ProfileDTO newDTO = new()
                    {
                        Id = profile.Id,
                        Type = profile.Type,
                        TypeDescription = profile.Type.ToString(),
                        IsActive = profile.IsActive,
                    };

                    profilesDTO.Add(newDTO);
                }

                return profilesDTO;
            };
        }

        public IList<PersonDTO> GetAllPersonsAssignedToProfile(ulong idProfile)
        {
            Profile? profile = _profileDAO.GetById(idProfile);

            if (profile is null) throw new Exception("The profile was not found");

            if (profile != null)
            {
                IEnumerable<User> users = _userDAO.GetAll().Where(u => u.IdProfile == idProfile);

                List<PersonDTO> personDTOs = new();

                foreach (User u in users)
                {
                    Person? currentPerson = _personDAO.GetAll().FirstOrDefault(p => p.IdUser == u.Id);

                    if (currentPerson is null) throw new Exception($"Person of linked to profile of id {u.Id} was not found");

                    PersonDTO newDTO = new()
                    {
                        Id = currentPerson.Id,
                        FullName = currentPerson.FullName,
                        CPF = currentPerson.CPF
                    };

                    personDTOs.Add(newDTO);
                }
                
                return personDTOs;
            }

            else throw new Exception();
        }

        public IList<LegalPersonDTO> GetAllLegalPersonsAssignedToProfile(ulong idProfile)
        {
            Profile? profile = _profileDAO.GetById(idProfile);

            if (profile is null) throw new Exception("The profile was not found");

            if (profile != null)
            {
                IEnumerable<User> users = _userDAO.GetAll().Where(u => u.IdProfile == idProfile);

                List<LegalPersonDTO> legalPersonDTOs = new();

                foreach (User u in users)
                {
                    LegalPerson? currentLegalPerson = _legalPersonDAO.GetAll().FirstOrDefault(l => l.IdUser == l.Id);

                    if (currentLegalPerson is null) throw new Exception($"Legal Person of linked to profile of id {u.Id} was not found");

                    LegalPersonDTO newDTO = new()
                    {
                        Id = currentLegalPerson.Id,
                        CompanyName = currentLegalPerson.CompanyName,
                        CNPJ = currentLegalPerson.CNPJ
                    };

                    legalPersonDTOs.Add(newDTO);
                }

                return legalPersonDTOs;
            }

            else throw new Exception();
        }

        public void AssignPersonToProfile(ulong idPerson, ulong idProfile)
        {
            Person? person = _personDAO.GetById(idPerson);

            if (person is null) throw new Exception("The person was not found");

            User? user = _userDAO.GetById(person.IdUser);

            if (user is null) throw new Exception("The user of this person was not found");

            Profile? profile = _profileDAO.GetById(idProfile);

            if (profile is null) throw new Exception("The profile was not found");

            if (person is not null && user is not null && profile is not null)
            {
                if (user.IdProfile.Equals(idProfile)) throw new Exception("The user of this person is already assigned to this profile");

                user.IdProfile = idProfile;
                user.Profile = profile;

                _userDAO.Update(user);
            }

            else throw new Exception();
        }

        public void AssignLegalPersonToProfile(ulong idLegalPerson, ulong idProfile)
        {
            LegalPerson? legalPerson = _legalPersonDAO.GetById(idLegalPerson);

            if (legalPerson is null) throw new Exception("The legal person was not found");

            User? user = _userDAO.GetById(legalPerson.IdUser);

            if (user is null) throw new Exception("The user of this legal person was not found");

            Profile? profile = _profileDAO.GetById(idProfile);

            if (profile is null) throw new Exception("The profile was not found");

            if (legalPerson is not null && profile is not null)
            {
                if (user.IdProfile.Equals(idProfile)) throw new Exception("The user of this legal person is already assigned to this profile");

                user.IdProfile = idProfile;
                user.Profile = profile;

                _userDAO.Update(user);
            }

            else throw new Exception();
        }

        public ProfileDTO CreateProfile(ProfileDTO profile)
        {
            Profile? registred = _profileDAO.GetAll().FirstOrDefault(p => p.Type == profile.Type);

            if (registred is not null) throw new Exception("This profile is already registred");

            if (registred is null)
            {
                Profile newEntity = new()
                {
                    Type = profile.Type,
                    IsActive = profile.IsActive
                };

                Profile newProfile = _profileDAO.Create(newEntity);

                ProfileDTO newDTO = new()
                {
                    Id = profile.Id,
                    Type = profile.Type,
                    TypeDescription = profile.Type.ToString(),
                    IsActive = profile.IsActive,
                };

                return newDTO;
            }

            else throw new Exception();
        }

        public void SwitchProfileActiveStatus(ulong idProfile)
        {
            Profile profile = _profileDAO.GetById(idProfile);

            if (profile is null) throw new NullReferenceException("The profile was not found");

            else _profileDAO.SwitchProfileActiveStatus(profile);
        }

        public void DeleteProfile(ulong idProfile)
        {
            Profile profile = _profileDAO.GetById(idProfile);

            if (profile is null) throw new NullReferenceException("The profile was not found");

            else _profileDAO.Delete(profile);
        }
    }
}
