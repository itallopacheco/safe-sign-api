using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using NLog;

using Safe_Sign.DTO.Credentials;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class CredentialsController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private static readonly Logger _loggerInfo = LogManager.GetLogger("loggerInfoCredentialsFile");
        private static readonly Logger _loggerError = LogManager.GetLogger("loggerErrorCredentialsFile");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="personRepository"></param>
        public CredentialsController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        /// <summary>
        /// Authenticate an user by username and password
        /// </summary>
        /// <param name="loginCredentials"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(LoginDTO loginCredentials)
        {
            try
            {
                // Person
                if (loginCredentials.userType == 0)
                {
                    CredentialsDTO credentialsDTO = _personRepository.GetPersonByCredentials(loginCredentials.Username, loginCredentials.Password);

                    var result = new JsonResult(credentialsDTO)
                    {
                        StatusCode = 200
                    };

                    _loggerInfo.Info("PESSOA FÍSICA LOGADA COM SUCESSO");
                    return result;
                }

                // Legal person
                else
                {
                    var result = new JsonResult("Currently its not possible to login with legal persons")
                    {
                        StatusCode = 403
                    };

                    _loggerInfo.Info("PESSOA JURÍDICA LOGADA COM SUCESSO");
                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new JsonResult("Error on login:\n" + ex.Message)
                {
                    StatusCode = 401
                };

                _loggerError.Error("ERRO AO LOGAR!");

                return result;
            }
        }
    }
}
