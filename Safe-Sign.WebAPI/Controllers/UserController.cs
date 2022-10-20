using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using NLog;

using Safe_Sign.DTO.Person;
using Safe_Sign.DTO.LegalPerson;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    // [Authorize]
    public class UserController : ControllerBase
    {   
        private readonly IPersonRepository _personRepository;
        private readonly ILegalPersonRepository _legalPersonRepository;
        private static readonly Logger _loggerInfo = LogManager.GetLogger("loggerInfoUserFile");
        private static readonly Logger _loggerError = LogManager.GetLogger("loggerErrorUserFile");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="personRepository"></param>
        /// <param name="legalPersonRepository"></param>
        public UserController(IPersonRepository personRepository, ILegalPersonRepository legalPersonRepository)
        {
            _personRepository = personRepository;
            _legalPersonRepository = legalPersonRepository;
        }

        /// <summary>
        /// Check for controller status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string HealthCheck() => "UserController is online, and working!";

        /// <summary>
        /// Get a person by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPersonById(ulong id)
        {
            try
            {
                PersonDTO person = _personRepository.GetPersonById(id);

                JsonResult result = new(person)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR PESSOA POR ID");
                return result;

            } 
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on get the specific person: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR PESSOA POR ID");
                return result;
            }
              
        }

        /// <summary>
        /// Get a legal person by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetLegalPersonById(ulong id)
        {
            try
            {
                LegalPersonDTO person = _legalPersonRepository.GetLegalPersonById(id);

                JsonResult result = new(person)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR PESSOA JURÍDICA POR ID");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on get the specific legal person:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR PESSOA JURÍDICA POR ID");
                return result;
            }
        }

        /// <summary>
        /// Get a person from a given name
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPersonByName(string fullname)
        {
            try
            {
                PersonDTO person = _personRepository.GetPersonByName(fullname);

                JsonResult result = new(person)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR PESSOA POR NOME");
                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on get the specific person:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR PESSOA POR NOME");
                return result;
            }
        }

        /// <summary>
        /// Get a person from a given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPersonByEmail(string email)
        {
            try
            {
                PersonDTO person = _personRepository.GetPersonByEmail(email);

                JsonResult result = new(person)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR PESSOA POR EMAIL");
                return result;

            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on get the specific person:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR PESSOA POR EMAIL");
                return result;
            }
        }

        /// <summary>
        /// Get a person from a given CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPersonByCPF(string cpf)
        {
            try
            {
                PersonDTO person = _personRepository.GetPersonByCPF(cpf);
                JsonResult result = new(person)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR PESSOA POR CPF");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on get the specific person:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR PESSOA POR CPF");
                return result;
            }
        }

        /// <summary>
        /// Get all persons
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllPersons()
        {
            try
            {
                IEnumerable<PersonDTO> persons = _personRepository.GetAllPersons();

                JsonResult result = new(persons)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR TODAS AS PESSOAS");
                return result;

            }
            catch(Exception ex)
            {
                // _log
                JsonResult result = new("Error on get all persons:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR TODAS AS PESSOAS");
                return result;
            }
        }

        /// <summary>
        /// Get all legal persons
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllLegalPersons()
        {
            try
            {
                IEnumerable<LegalPersonDTO> persons = _legalPersonRepository.GetAllLegalPersons();

                JsonResult result = new(persons)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR TODAS AS PESSOAS JURÍDICAS");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on get all legal persons:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR TODAS AS PESSOAS JURÍDICAS");
                return result;
            }
        }

        /// <summary>
        /// Create a new person
        /// </summary>
        /// <param name="personDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreatePerson(PersonDTO personDTO)
        {
            try
            {
                PersonDTO person = _personRepository.CreatePerson(personDTO);

                JsonResult result = new(person)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO CRIAR PESSOA");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on create a new person:\n" + ex.Message)
                {
                    StatusCode = 500
                };
               
                _loggerError.Error("ERRO AO CRIAR PESSOA");
                return result;
            }
        }

        /// <summary>
        /// Create a new legal person
        /// </summary>
        /// <param name="legalPersonDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateLegalPerson(LegalPersonDTO legalPersonDTO)
        {
            try
            {
                LegalPersonDTO person = _legalPersonRepository.CreateLegalPerson(legalPersonDTO);

                JsonResult result = new(person)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO CRIAR PESSOA JURÍDICA");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on create a new legal person:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO CRIAR PESSOA JURÍDICA");
                return result;
            }
        }

        /// <summary>
        /// Update a person
        /// </summary>
        /// <param name="personDTO"></param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult UpdatePerson(UpdatePersonDTO personDTO)
        {
            try
            {
                PersonDTO person = _personRepository.UpdatePerson(personDTO);

                JsonResult result = new(person)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO ATUALIZAR PESSOA");
                return result;
            } 
            catch(Exception ex)
            {
                // _log
                JsonResult result = new("Error on update the given person:\n" + ex.Message)
                {
                    StatusCode = 500
                }; 

                _loggerError.Error("ERRO AO ATUALIZAR PESSOA");
                return result;
            }
        }

        /// <summary>
        /// Update a legal person
        /// </summary>
        /// <param name="personDTO"></param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult UpdateLegalPerson(UpdateLegalPersonDTO personDTO)
        {
            try
            {
                LegalPersonDTO theme = _legalPersonRepository.UpdateLegalPerson(personDTO);

                JsonResult result = new(theme)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO ATUALIZAR PESSOA JURÍDICA");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on update the given legal person:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO ATUALIZAR PESSOA JURÍDICA");
                return result;
            }
        }
        
        [HttpPost]
        public IActionResult SendEmail(ulong idUser)
        {
            try
            {
                string email = _personRepository.SendEmail(idUser);

                JsonResult result = new(email)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO ENVIAR EMAIL");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on send a new email:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO ENVIAR EMAIL");
                return result;
            }
        }
        /// <summary>
        /// Delete a person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeletePerson(ulong id)
        {
            try
            {
                _personRepository.DeletePerson(id);

                _loggerInfo.Info("SUCESSO AO DELETAR PESSOA");
                return new NoContentResult();
            } 
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on delete the giving person:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO DELETAR PESSOA JURÍDICA");
                return result;
            }
        }

        /// <summary>
        /// Delete a legal person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteLegalPerson(ulong id)
        {

            try
            {
                _legalPersonRepository.DeleteLegalPerson(id);

                _loggerInfo.Info("SUCESSO AO DELETAR PESSOA JURÍDICA");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on delete the giving legal person:\n" + ex.Message) 
                { 
                    StatusCode = 500 
                };

                _loggerError.Error("ERRO AO DELETAR PESSOA JURÍDICA");
                return result;
            }
        }
    }
}
