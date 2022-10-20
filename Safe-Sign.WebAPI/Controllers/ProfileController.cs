using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Safe_Sign.DTO.Person;
using Safe_Sign.DTO.Profile;
using Safe_Sign.DTO.LegalPerson;
using Safe_Sign.Repository.Interfaces;

using NLog;

namespace Safe_Sign.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    // [Authorize(Roles = "0")] // Administrator
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;
        private static readonly Logger _loggerInfo = LogManager.GetLogger("loggerInfoProfileFile");
        private static readonly Logger _loggerError = LogManager.GetLogger("loggerErrorProfileFile");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="profileRepository"></param>
        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        /// <summary>
        /// Check for controller status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string HealthCheck() => "ProfileController is online and working!";

        /// <summary>
        /// Get a profile by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProfileById(ulong id)
        {
            try
            {
                ProfileDTO profile = _profileRepository.GetProfileById(id);

                JsonResult result = new(profile)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR PERFIL POR ID");

                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on get the specific profile:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR PERFIL POR ID");
                return result;
            }
        }

        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllProfiles()
        {
            try
            {
                IList<ProfileDTO> profiles = _profileRepository.GetAllProfiles();

                JsonResult result = new(profiles)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR PERFIS");
                return result;

            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on get all profiles:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR PERFÍS");
                return result;
            }
        }

        /// <summary>
        /// Get all persons assigned to a giving profile
        /// </summary>
        /// <param name="idProfile"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllPersonsAssignedToProfile(ulong idProfile)
        {
            try
            {
                IList<PersonDTO> persons = _profileRepository.GetAllPersonsAssignedToProfile(idProfile);

                JsonResult result = new(persons)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO GARANTIR PERMISSAO A PERFIL");
                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on get persons assigned to this profile:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO GARANTIR PERMISSAO A PERFIL");
                return result;
            }
        }

        /// <summary>
        /// Get all legal persons assigned to a giving profile
        /// </summary>
        /// <param name="idProfile"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllLegalPersonsAssignedToProfile(ulong idProfile)
        {
            try
            {
                IList<LegalPersonDTO> legalPersons = _profileRepository.GetAllLegalPersonsAssignedToProfile(idProfile);

                JsonResult result = new(legalPersons)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR PESSOA DESIGNADA AO PERFIL");
                return result;
            }
            catch (Exception ex)
            {

                JsonResult result = new("Error on get legal persons assigned to this profile:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR PESSOA DESIGNADA AO PERFIL");
                return result;
            }
        }

        /// <summary>
        /// Create a new profile
        /// </summary>
        /// <param name="profileDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateProfile(ProfileDTO profileDTO)
        {
            try
            {
                ProfileDTO profile = _profileRepository.CreateProfile(profileDTO);

                JsonResult result = new(profile)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO CRIAR PERFIL");
                return result;

            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on create a new profile:\n" + ex.Message)
                {
                    StatusCode = 500
                };
                _loggerError.Error("ERRO AO CRIAR PERFIL");
                return result;
            }
        }

        /// <summary>
        /// Assign a giving person to the target profile
        /// </summary>
        /// <param name="idPerson"></param>
        /// <param name="idProfile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AssignPersonToProfile(ulong idPerson, ulong idProfile)
        {
            try
            {
                _profileRepository.AssignPersonToProfile(idPerson, idProfile);

                _loggerInfo.Info("SUCESSO AO LIGAR PESSOA A PERFIL");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error assign the giving person to the target profile:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO LIGAR PESSOA A PERFIL");
                return result;
            }
        }

        /// <summary>
        /// Assign a giving legal person to the target profile
        /// </summary>
        /// <param name="idLegalPerson"></param>
        /// <param name="idProfile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AssignLegalPersonToProfile(ulong idLegalPerson, ulong idProfile)
        {
            try
            {
                _profileRepository.AssignLegalPersonToProfile(idLegalPerson, idProfile);

                _loggerInfo.Info("SUCESSO AO LIGAR PESSOA JURÍDICA A PERFIL");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error assign the giving legal person to the target profile:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO LIGAR PESSOA JURÍDICA A PERFIL");
                return result;
            }
        }

        /// <summary>
        /// Switch active status from a giving profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult SwitchProfileActiveStatus(ulong id)
        {
            try
            {
                _profileRepository.SwitchProfileActiveStatus(id);
                _loggerInfo.Info("SUCESSO AO MUDAR STATUS DE ATIVIDADE DO PERFIL");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on switch active status from the giving profile:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO MUDAR STATUS DE ATIVIDADE DO PERFIL");
                return result;
            }
        }

        /// <summary>
        /// Delete a profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteProfile(ulong id)
        {
            try
            {
                _profileRepository.DeleteProfile(id);

                _loggerInfo.Info("SUCESSO AO DELETAR PERFIL");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on delete the giving profile:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("FALHA AO DELETAR PERFIL");
                return result;
            }
        }
    }
}
