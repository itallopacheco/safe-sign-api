using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Safe_Sign.DTO.Document;
using Safe_Sign.DTO.Signature;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    // [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IDocumentRepository _documentRepository;
        private readonly ISignatureRepository _signatureRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="documentRepository"></param>
        /// <param name="signatureRepository"></param>
        public DocumentController(IWebHostEnvironment environment, IDocumentRepository documentRepository, ISignatureRepository signatureRepository)
        {
            _environment = environment;
            _documentRepository = documentRepository;
            _signatureRepository = signatureRepository;
        }

        [HttpGet]
        public string HealthCheck() => "I'm alive and ready to work with documents";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                IList<DocumentDTO> documents = _documentRepository.GetAll();

                JsonResult result = new(documents)
                {
                    StatusCode = 200
                };

                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new(ex)
                {
                    StatusCode = 500
                };

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateDocument([FromForm] DocumentDTO document)
        {
            try
            {
                DocumentDTO documentDTO = _documentRepository.CreateDocument(document, _environment.WebRootPath);

                JsonResult result = new(documentDTO)
                {
                    StatusCode = 200
                };

                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on create a new Document:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ValidateDocument(string token)
        {
            try
            {
                bool isDocumentValid = _documentRepository.GetDocumentByToken(token);

                if (isDocumentValid)
                {
                    JsonResult result = new(isDocumentValid) { StatusCode = 200 };
                    return result;
                }
                else
                {
                    JsonResult result = new(isDocumentValid) { StatusCode = 500 };
                    return result;
                }

                
            }
            catch (Exception ex)
            {

                JsonResult result = new("Error on validate a document:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult FinishDocument(ulong idDocument)
        {
            try
            {
                _documentRepository.SignDocument(idDocument);

                return NoContent();
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on finish the document:\n" + ex.Message)
                {
                    StatusCode = 500
                };

                return result;
            }
        }
    }
}
