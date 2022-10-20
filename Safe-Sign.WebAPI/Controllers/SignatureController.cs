using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Safe_Sign.DTO.Signature;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    // [Authorize]
    public class SignatureController : ControllerBase
    {
        private readonly ISignatureRepository _signatureRepository;

        public SignatureController(ISignatureRepository signatureRepository)
        {
            _signatureRepository = signatureRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signatureDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateSignature(SignatureDTO signatureDTO)
        {
            try
            {
                SignatureDTO signature = _signatureRepository.CreateSignature(signatureDTO);       

                JsonResult result = new(signature)
                {
                    StatusCode = 200
                };

                return result;
            }
            catch(Exception ex)
            {
                JsonResult result = new(ex)
                {
                    StatusCode = 500
                };

                return result;
            }
        }
    }
}
