using Safe_Sign.Util;

using Safe_Sign.DAL.DAO;
using Safe_Sign.DTO.User;
using Safe_Sign.DAL.Models;
using Safe_Sign.DTO.Person;
using Safe_Sign.DTO.Credentials;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDAO _personDAO;
        private readonly UserDAO _userDAO;

        public PersonRepository(PersonDAO personDAO, UserDAO userDAO)
        {
            _personDAO = personDAO;
            _userDAO = userDAO;
        }

        public PersonDTO GetPersonById(ulong idPerson)
        {
            Person person = _personDAO.GetById(idPerson);

            if (person is null) throw new NullReferenceException("This person was not found");
            
            User personUser = _userDAO.GetById(person.IdUser);

            UserDTO userDTO = new()
            {
                Id = personUser.Id,
                UserType = personUser.UserType,
                Username = personUser.Username,
                Password = personUser.Password,
                Email = personUser.Email,
                EmailVerified = personUser.EmailVerified,
                IsActive = personUser.IsActive,
                IdProfile = personUser.IdProfile
            };

            PersonDTO finalDTO = new()
            {
                Id = person.Id,
                FullName = person.FullName,
                CPF = person.CPF,
                BirthDate = person.BirthDate,
                MotherName = person.MotherName,
                PrimaryPhone = person.PrimaryPhone,
                SecondaryPhone = person.SecondaryPhone,
                User = userDTO
            };

            return finalDTO;
        }

        public PersonDTO GetPersonByName(string name)
        {
            Person person = _personDAO.GetByName(name);

            if (person is null) throw new NullReferenceException("This person was not found");

            User personUser = _userDAO.GetById(person.IdUser);

            if (personUser is null) throw new NullReferenceException("The user information of this person was not found");

            UserDTO userDTO = new()
            {
                Id = personUser.Id,
                UserType = personUser.UserType,
                Username = personUser.Username,
                Password = personUser.Password,
                Email = personUser.Email,
                EmailVerified = personUser.EmailVerified,
                IsActive = personUser.IsActive,
                IdProfile = personUser.IdProfile
            };

            PersonDTO finalDTO = new()
            {
                Id = person.Id,
                FullName = person.FullName,
                CPF = person.CPF,
                BirthDate = person.BirthDate,
                MotherName = person.MotherName,
                PrimaryPhone = person.PrimaryPhone,
                SecondaryPhone = person.SecondaryPhone,
                User = userDTO
            };

            return finalDTO;
        }

        public PersonDTO GetPersonByEmail(string email)
        {
            Person person = _personDAO.GetByEmail(email);

            if (person is null) throw new NullReferenceException("This person was not found");

            User personUser = _userDAO.GetById(person.IdUser);

            if (personUser is null) throw new NullReferenceException("The user information of this person was not found");

            UserDTO userDTO = new()
            {
                Id = personUser.Id,
                UserType = personUser.UserType,
                Username = personUser.Username,
                Password = personUser.Password,
                Email = personUser.Email,
                EmailVerified = personUser.EmailVerified,
                IsActive = personUser.IsActive,
                IdProfile = personUser.IdProfile
            };

            PersonDTO finalDTO = new()
            {
                Id = person.Id,
                FullName = person.FullName,
                CPF = person.CPF,
                BirthDate = person.BirthDate,
                MotherName = person.MotherName,
                PrimaryPhone = person.PrimaryPhone,
                SecondaryPhone = person.SecondaryPhone,
                User = userDTO
            };

            return finalDTO;
        }

        public CredentialsDTO GetPersonByCredentials(string username, string password)
        {
            if (username is null || password is null) throw new ArgumentNullException("Error, Username or password parameters are null");

            else
            {
                User? personUser = _userDAO.GetAll().FirstOrDefault(u => u.Username.ToLower().Trim() == username.ToLower().Trim() && u.Password == password);
                
                if (personUser is null) throw new NullReferenceException("The user information of this person was not found");

                Person? person = _personDAO.GetAll().FirstOrDefault(p => p.IdUser == personUser.Id);

                if (person is null) throw new NullReferenceException("This person was not found");

                UserDTO userDTO = new()
                {
                    Id = personUser.Id,
                    UserType = personUser.UserType,
                    Username = personUser.Username,
                    Password = personUser.Password,
                    Email = personUser.Email,
                    EmailVerified = personUser.EmailVerified,
                    IsActive = personUser.IsActive,
                    IdProfile = personUser.IdProfile
                };

                PersonDTO finalDTO = new()
                {
                    Id = person.Id,
                    FullName = person.FullName,
                    CPF = person.CPF,
                    BirthDate = person.BirthDate,
                    MotherName = person.MotherName,
                    PrimaryPhone = person.PrimaryPhone,
                    SecondaryPhone = person.SecondaryPhone,
                    User = userDTO
                };

                string token = UserAuthenticationTools.GenerateToken(userDTO);

                CredentialsDTO credentialsDTO = new()
                {
                    Username = userDTO.Username,
                    Email = userDTO.Email,
                    Token = token
                };

                return credentialsDTO;
            }
        }

        /// <summary>
        /// Get a person using CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public PersonDTO GetPersonByCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) throw new ArgumentNullException("Error, CPF parameter is null");

            Person person = _personDAO.GetByCPF(cpf);

            if (person is null) throw new NullReferenceException("This person was not found");

            User personUser = _userDAO.GetById(person.IdUser);

            if (personUser is null) throw new NullReferenceException("The user information of this person was not found");

            UserDTO userDTO = new()
            {
                Id = personUser.Id,
                UserType = personUser.UserType,
                Username = personUser.Username,
                Password = personUser.Password,
                Email = personUser.Email,
                EmailVerified = personUser.EmailVerified,
                IsActive = personUser.IsActive,
                IdProfile = personUser.IdProfile
            };

            PersonDTO finalDTO = new()
            {
                Id = person.Id,
                FullName = person.FullName,
                CPF = person.CPF,
                BirthDate = person.BirthDate,
                MotherName = person.MotherName,
                PrimaryPhone = person.PrimaryPhone,
                SecondaryPhone = person.SecondaryPhone,
                User = userDTO
            };

            return finalDTO;
        }

        /// <summary>
        /// Get all persons created
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IList<PersonDTO> GetAllPersons()
        {
            IEnumerable<Person> persons = _personDAO.GetAll();

            if (persons is null) throw new ArgumentNullException("Error on search for all persons");

            List<PersonDTO> personsDTO = new();

            foreach (Person p in persons)
            {
                User currentUser = _userDAO.GetById(p.IdUser);

                if (currentUser is null) throw new NullReferenceException($"Error on search for user information of person of id {p.Id}");

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

                PersonDTO newDTO = new()
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    CPF = p.CPF,
                    BirthDate = p.BirthDate,
                    MotherName = p.MotherName,
                    PrimaryPhone = p.PrimaryPhone,
                    SecondaryPhone = p.SecondaryPhone,
                    User = userDTO
                };

                personsDTO.Add(newDTO);
            }

            return personsDTO;
        }

        /// <summary>
        /// Create a new Person
        /// </summary>
        /// <param name="newPerson"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="Exception"></exception>
        public PersonDTO CreatePerson(PersonDTO newPerson)
        {
            Person? registred = _personDAO.GetAll().FirstOrDefault(p => p.CPF == newPerson.CPF);

            bool isValidCPF = FieldsValidationTools.ValidationCPF(newPerson.CPF);

            if (registred is null && isValidCPF)
            {
                User newUser = new()
                {
                    Username = newPerson.User.Username,
                    Password = newPerson.User.Password,
                    Email = newPerson.User.Email
                };

                User _user = _userDAO.Create(newUser);

                Person newEntity = new()
                {
                    FullName = newPerson.FullName,
                    CPF = newPerson.CPF,
                    BirthDate = newPerson.BirthDate,
                    MotherName = newPerson.MotherName,
                    PrimaryPhone = newPerson.PrimaryPhone,
                    SecondaryPhone = newPerson.SecondaryPhone,
                    IdUser = _user.Id
                };

                Person _person = _personDAO.Create(newEntity);

                UserDTO userDTO = new()
                {
                    Id = _user.Id,
                    UserType = _user.UserType,
                    Username = _user.Username,
                    Password = _user.Password,
                    Email = _user.Email,
                    EmailVerified = _user.EmailVerified,
                    IsActive = _user.IsActive,
                    IdProfile = _user.IdProfile
                };

                PersonDTO finalDTO = new()
                {
                    Id = _person.Id,
                    FullName = _person.FullName,
                    CPF = _person.CPF,
                    BirthDate = _person.BirthDate,
                    MotherName = _person.MotherName,
                    PrimaryPhone = _person.PrimaryPhone,
                    SecondaryPhone = _person.SecondaryPhone,
                    User = userDTO
                };

                return finalDTO;
            }

            else throw new Exception("This person is already registred");
        }
        
        /// <summary>
        /// Update a Person already created
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public PersonDTO UpdatePerson(UpdatePersonDTO person)
        {
            Person oldPerson = _personDAO.GetById(person.Id);

            if (oldPerson is null) throw new NullReferenceException("This person was not found");

            User oldUser = _userDAO.GetById(oldPerson.IdUser);

            if (oldUser is null) throw new NullReferenceException("The user information of this legal person was not found");

            oldPerson.PrimaryPhone = (!string.IsNullOrEmpty(person.PrimaryPhone) && 
                                          !oldPerson.PrimaryPhone.Trim().Equals(person.PrimaryPhone.Trim())) ?
                                            person.PrimaryPhone : 
                                                oldPerson.PrimaryPhone;

            oldPerson.SecondaryPhone = (!string.IsNullOrEmpty(person.SecondaryPhone) && 
                                       !oldPerson.SecondaryPhone.Trim().Equals(person.SecondaryPhone.Trim())) ? 
                                            person.SecondaryPhone : 
                                                oldPerson.SecondaryPhone;

            Person _person = _personDAO.Update(oldPerson);

            if (person.User is not null)
            {
                oldUser.Email = (!string.IsNullOrEmpty(person.User.Email) && 
                                 !oldUser.Email.Equals(person.User.Email)) ? 
                                    person.User.Email : 
                                        oldUser.Email;

                oldUser.Password = (!string.IsNullOrEmpty(person.User.Password) && 
                                    !oldUser.Password.Equals(person.User.Password)) ? 
                                        person.User.Password : 
                                        oldUser.Password;

                _userDAO.Update(oldUser);
            }

            User userAfterUpdate = _userDAO.GetById(oldPerson.IdUser);

            UserDTO userDTO = new()
            {
                Id = userAfterUpdate.Id,
                UserType = userAfterUpdate.UserType,
                Username = userAfterUpdate.Username,
                Password = userAfterUpdate.Password,
                Email = userAfterUpdate.Email,
                EmailVerified = userAfterUpdate.EmailVerified,
                IsActive = userAfterUpdate.IsActive,
                IdProfile = userAfterUpdate.IdProfile
            };

            PersonDTO finalDTO = new()
            {
                Id = _person.Id,
                FullName = _person.FullName,
                CPF = _person.CPF,
                BirthDate = _person.BirthDate,
                MotherName = _person.MotherName,
                User = userDTO
            };

            return finalDTO;
        }
        
        /// <summary>
        /// Send Email Passing the Person ID
        /// </summary>
        /// <param name="idPerson"></param>
        /// <returns></returns>
        public string SendEmail(ulong idPerson)
        {
            PersonDTO personDTO = GetPersonById(idPerson);

            User user = _userDAO.GetById(idPerson);

            string CPFEncoded = EmailTools.GenerateCode(personDTO.CPF);

            EmailTools.SendEmail(user.Email, CPFEncoded, personDTO.FullName, user.Username);

            return user.Email;
        }

        /// <summary>
        /// Switch the status of the EmailVerified passing an Encoded CPF
        /// </summary>
        /// <param name="toDecode"></param>
        public void SwitchUserEmailStatus(string toDecode)
        {
            string CPF = EmailTools.DeCode(toDecode);

            PersonDTO oldPersonDTO = GetPersonByCPF(CPF);

            _userDAO.SwitchUserEmailState(oldPersonDTO.Id);
        }

        /// <summary>
        /// Switch User Status passing a Person Id
        /// </summary>
        /// <param name="idPerson"></param>
        /// <returns></returns>
        public bool SwitchUserActiveStatus(ulong idPerson)
        {
            User user = _userDAO.GetById(idPerson);

            if (user is null) throw new NullReferenceException("The user was not found");

            if (user.EmailVerified)
            {
                _userDAO.SwitchUserActiveState(idPerson);
                return true;
            }

            return false;
        }

        /// <summary>
        ///  Delete a Person
        /// </summary>
        /// <param name="idPerson"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void DeletePerson(ulong idPerson)
        {
            Person person = _personDAO.GetById(idPerson);

            if (person is null) throw new NullReferenceException("This person was not found");

            _personDAO.Delete(person);

            User personUser = _userDAO.GetById(person.IdUser);

            if (personUser is null) throw new NullReferenceException("The user information of this person was not found");

            _userDAO.Delete(personUser);
        }
    }
}
