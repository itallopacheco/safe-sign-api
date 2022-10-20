using Microsoft.AspNetCore.Mvc;

using NLog;

using Safe_Sign.Util;
using Safe_Sign.DTO.SGP;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    // [Authorize]
    public class SGPController : ControllerBase
    {
        private static readonly Logger _loggerInfo = LogManager.GetLogger("loggerInfoSgpFile");
        private static readonly Logger _loggerError = LogManager.GetLogger("loggerErrorSgpFile");

        private readonly ISGPRepository _SGPRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public SGPController(ISGPRepository SGPRepository) 
        {
            _SGPRepository = SGPRepository;
        }

        /// <summary>
        /// Check for controller status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string HealthCheck() => "SGPController is online and working!";

        /// <summary>
        /// Check for SGP integration status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IsSGPListening()
        {
            try
            {
                bool active = SGPTools.TestConnectionWithSGP();

                var result = new JsonResult(active)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO ESTABELECER CONEXAO COM A API DO SGP");
                return result;
            }
            catch (Exception ex)
            {
                var result = new JsonResult("Error on connect with SGP API:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO ESTABELECER CONEXAO COM A API DO SGP");
                return result;
            }
        }

        /// <summary>
        /// Log in an user into the SGP
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AuthenticateSGPUser(SGPSignInDTO user)
        {
            try
            {
                SGPTools.SignInOnSGP(user);

                _loggerInfo.Info("SUCESSO AO AUTENTICAR USUÁRIO");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                var result = new JsonResult("Error on authenticate this user on SGP:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO AUTENTICAR USUÁRIO");
                return result;
            }
        }

        /// <summary>
        /// Collect SGP user data from integration
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        [HttpPost]
        public IActionResult CollectSGPUserData(SGPSignInDTO user)
        {
            try
            {
                string token = SGPTools.SignInOnSGP(user);

                if (string.IsNullOrEmpty(token)) throw new NullReferenceException();

                SGPUserDTO? userDate = SGPTools.GetSGPUserData(user, token);

                if (userDate is null) throw new NullReferenceException();

                else
                {
                    var result = new JsonResult(userDate)
                    {
                        StatusCode = 200
                    };

                    _loggerInfo.Info("SUCESSO AO OBTER DADOS DE USUÁRIO DO SGP");
                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new JsonResult("Error on get user data from SGP:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO OBTER DADOS DE USUÁRIO DO SGP");
                return result;
            }
        }

        /// <summary>
        /// Sign documents coming from SGP
        /// </summary>
        /// <param name="documentToSign"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SignDocument([FromForm] SGPDocumentToSign documentToSign)
        // public IActionResult SignDocument([FromForm] IFormFile sgpFile, [FromForm] string userLogin)
        {
            if (documentToSign.SgpFile.Length > 0)
            {
                try
                {
                    SGPSignedDocumentDTO signedDocument = _SGPRepository.SignSGPDocument(documentToSign.SgpFile, documentToSign.UserLogin);

                    return Ok(signedDocument);
                }
                catch (Exception ex)
                {
                    var result = new JsonResult("Error on sign document coming from SGP:\n" + ex.Message)
                    {
                        StatusCode = 500
                    };

                    return result;
                }
            }

            else
            {
                var result = new JsonResult("Error on process the file. The file are corrupted")
                {
                    StatusCode = 500
                };

                return result;
            }
        }
    }
}
