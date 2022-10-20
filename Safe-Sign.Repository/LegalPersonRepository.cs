using Safe_Sign.DAL.DAO;
using Safe_Sign.DTO.User;
using Safe_Sign.DAL.Models;
using Safe_Sign.DAL.DAO.Base;
using Safe_Sign.DTO.LegalPerson;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.Repository
{
    public class LegalPersonRepository: ILegalPersonRepository
    {
        private readonly IDAO<LegalPerson> _legalPersonDAO;
        private readonly UserDAO _userDAO;

        public LegalPersonRepository(IDAO<LegalPerson> LegalPersonDAO, UserDAO userDAO)
        {
            _legalPersonDAO = LegalPersonDAO;
            _userDAO = userDAO;
        }

        public LegalPersonDTO GetLegalPersonById(ulong idPerson)
        {
            LegalPerson? legalPerson = _legalPersonDAO.GetById(idPerson);

            if (legalPerson is null) throw new NullReferenceException("The legal person was not found");

            User legalPersonUser = _userDAO.GetById(legalPerson.IdUser);
            
            if (legalPersonUser is null) throw new NullReferenceException("The user information of this legal person was not found");

            UserDTO userDTO = new()
            {
                Id = legalPersonUser.Id,
                UserType = legalPersonUser.UserType,
                Username = legalPersonUser.Username,
                Password = legalPersonUser.Password,
                Email = legalPersonUser.Email,
                EmailVerified = legalPersonUser.EmailVerified,
                IsActive = legalPersonUser.IsActive,
                IdProfile = legalPersonUser.IdProfile
            };

            LegalPersonDTO newLegalPersonDTO = new()
            {
                Id = legalPerson.Id,
                CompanyName = legalPerson.CompanyName,
                CNPJ = legalPerson.CNPJ,
                LegalAgent = legalPerson.LegalAgent,
                PrimaryPhone = legalPerson.PrimaryPhone,
                SecondaryPhone = legalPerson.SecondaryPhone,
                User = userDTO
            };

            return newLegalPersonDTO;
        }

        public IList<LegalPersonDTO> GetAllLegalPersons()
        {
            IEnumerable<LegalPerson> legalPersons = _legalPersonDAO.GetAll();
            
            if (legalPersons is null) throw new NullReferenceException("Error on search for all legal persons");

            List<LegalPersonDTO> legalPersonsDTO = new();

            foreach (LegalPerson legalPerson in legalPersons)
            {
                User currentUser = _userDAO.GetById(legalPerson.IdUser);

                if (currentUser is null) throw new NullReferenceException($"Error on search for user information of legal person of id {legalPerson.Id}");

                UserDTO userDTO = new()
                {
                    Id = currentUser.Id,
                    UserType = currentUser.UserType,
                    Username = currentUser.Username,
                    Password = currentUser.Password,
                    Email = currentUser.Email,
                    EmailVerified = currentUser.EmailVerified,
                    IsActive = currentUser.IsActive,
                    IdProfile = currentUser.IdProfile
                };

                LegalPersonDTO newLegalPersonDTO = new()
                {
                    Id = legalPerson.Id,
                    CompanyName = legalPerson.CompanyName,
                    CNPJ = legalPerson.CNPJ,
                    LegalAgent = legalPerson.LegalAgent,
                    PrimaryPhone = legalPerson.PrimaryPhone,
                    SecondaryPhone = legalPerson.SecondaryPhone,
                    User = userDTO
                };

                legalPersonsDTO.Add(newLegalPersonDTO);
            }

            return legalPersonsDTO;
        }

        public static bool IsInformationsWrong(LegalPersonDTO person)
        {
            if (string.IsNullOrEmpty(person.CNPJ) || string.IsNullOrEmpty(person.LegalAgent) || string.IsNullOrEmpty(person.CompanyName))
                return true;

            return false;
        }

        private LegalPersonDTO CopyInformationDTO(LegalPerson legalPerson, User legalPersonUser)
        {
            if (legalPersonUser is null) throw new NullReferenceException($"Error on search for user information of legal person of id {legalPerson.Id}");

            UserDTO userDTO = new()
            {
                Id = legalPersonUser.Id,
                UserType = legalPersonUser.UserType,
                Username = legalPersonUser.Username,
                Password = legalPersonUser.Password,
                Email = legalPersonUser.Email,
                EmailVerified = legalPersonUser.EmailVerified,
                IsActive = legalPersonUser.IsActive,
                IdProfile = legalPersonUser.IdProfile
            };

            LegalPersonDTO aux = new()
            {
                Id = legalPerson.Id,
                CompanyName = legalPerson.CompanyName,
                CNPJ = legalPerson.CNPJ,
                LegalAgent = legalPerson.LegalAgent,
                PrimaryPhone = legalPerson.PrimaryPhone,
                SecondaryPhone = legalPerson.SecondaryPhone,
                User = userDTO
            };

            return aux;
        }
        
        public LegalPersonDTO CreateLegalPerson(LegalPersonDTO legalPerson)
        {
            if (IsInformationsWrong(legalPerson)) throw new NullReferenceException("Invalid legal person data");

            LegalPerson? registred = _legalPersonDAO.GetAll().FirstOrDefault(t => t.CompanyName.ToLower().Trim() == legalPerson.CompanyName.ToLower().Trim());

            if (registred is null)
            {
                User newUser = new()
                {
                    Username = legalPerson.User.Username,
                    Password = legalPerson.User.Password,
                    Email = legalPerson.User.Email
                };

                User _user = _userDAO.Create(newUser);

                LegalPerson newLegalPerson = new()
                {
                    CompanyName = legalPerson.CompanyName,
                    CNPJ = legalPerson.CNPJ,
                    LegalAgent = legalPerson.LegalAgent,
                    PrimaryPhone = legalPerson.PrimaryPhone,
                    SecondaryPhone = legalPerson.SecondaryPhone,
                    IdUser = _user.Id
                };

                LegalPerson _person = _legalPersonDAO.Create(newLegalPerson);

                LegalPersonDTO newLegalPersonDTO = new();

                newLegalPersonDTO = CopyInformationDTO(_person, _user);

                return newLegalPersonDTO;
            }

            else throw new Exception("This legal person is already registred");
        }

        public LegalPersonDTO UpdateLegalPerson(UpdateLegalPersonDTO legalPerson)
        {
            LegalPerson oldLegalPerson = _legalPersonDAO.GetById(legalPerson.Id);

            if (oldLegalPerson is null) throw new NullReferenceException("The legal person was not found");

            User oldUser = _userDAO.GetById(oldLegalPerson.IdUser);

            if (oldUser is null) throw new NullReferenceException("The user information of this legal person was not found");

            oldLegalPerson.CompanyName = (!string.IsNullOrEmpty(legalPerson.CompanyName) && 
                                         !oldLegalPerson.CompanyName.Equals(legalPerson.CompanyName)) ? 
                                            legalPerson.CompanyName : 
                                                oldLegalPerson.CompanyName;

            oldLegalPerson.CNPJ = (!string.IsNullOrEmpty(legalPerson.CNPJ) && 
                                   !oldLegalPerson.CNPJ.Equals(legalPerson.CNPJ)) ? 
                                    legalPerson.CNPJ : 
                                        oldLegalPerson.CNPJ;

            oldLegalPerson.LegalAgent = (!string.IsNullOrEmpty(legalPerson.LegalAgent) && 
                                         !oldLegalPerson.LegalAgent.Equals(legalPerson.LegalAgent)) ? 
                                            legalPerson.LegalAgent : 
                                                oldLegalPerson.LegalAgent;
            
            oldLegalPerson.PrimaryPhone = (!string.IsNullOrEmpty(legalPerson.PrimaryPhone) && 
                                           !oldLegalPerson.PrimaryPhone.Equals(legalPerson.PrimaryPhone)) ? 
                                            legalPerson.PrimaryPhone : 
                                                oldLegalPerson.PrimaryPhone;
            
            oldLegalPerson.SecondaryPhone = (!string.IsNullOrEmpty(legalPerson.SecondaryPhone) && 
                                             !oldLegalPerson.SecondaryPhone.Equals(legalPerson.SecondaryPhone)) ? 
                                                legalPerson.SecondaryPhone : 
                                                    oldLegalPerson.SecondaryPhone;

            LegalPerson _legalPerson = _legalPersonDAO.Update(oldLegalPerson);

            if (legalPerson.User is not null)
            {
                oldUser.Email =  (!string.IsNullOrEmpty(legalPerson.User.Email) && 
                                  !oldUser.Email.Equals(legalPerson.User.Email)) ? 
                                    legalPerson.User.Email : 
                                        oldUser.Email;

                oldUser.Password = (!string.IsNullOrEmpty(legalPerson.User.Password) && 
                                    !oldUser.Password.Equals(legalPerson.User.Password)) ? 
                                        legalPerson.User.Password : 
                                        oldUser.Password;

                _userDAO.Update(oldUser);
            }

            User userAfterUpdate = _userDAO.GetById(oldLegalPerson.IdUser);

            LegalPersonDTO newPerson = CopyInformationDTO(_legalPerson, userAfterUpdate);

            return newPerson;
        }

        public void DeleteLegalPerson(ulong idLegalPerson)
        {
            LegalPerson legalPerson = _legalPersonDAO.GetById(idLegalPerson);

            if (legalPerson is null) throw new NullReferenceException("This legal person was not found");

            _legalPersonDAO.Delete(legalPerson);

            User legalPersonUser = _userDAO.GetById(legalPerson.IdUser);

            if (legalPersonUser is null) throw new NullReferenceException("The user information of this legal person was not found");

            _userDAO.Delete(legalPersonUser.Id);
        }
    }
}
