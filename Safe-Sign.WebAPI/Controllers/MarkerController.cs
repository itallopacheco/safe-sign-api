using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Safe_Sign.DTO.Marker;
using Safe_Sign.Repository.Interfaces;

using NLog;

namespace Safe_Sign.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(Roles = "0")] // Administrator
    public class MarkerController : ControllerBase
    {
        private readonly IMarkerRepository _markerRepository;
        private static readonly Logger _loggerInfo = LogManager.GetLogger("loggerInfoMarkerFile");
        private static readonly Logger _loggerError = LogManager.GetLogger("loggerErrorMarkerFile");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="markerRepository"></param>
        public MarkerController(IMarkerRepository markerRepository)
        {
            _markerRepository = markerRepository;
        }

        /// <summary>
        /// Check for controller status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string HealthCheck() => "MarkerController is online and working!";

        /// <summary>
        /// Get a marker by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMarkerById(ulong id)
        {
            try
            {
                MarkerDTO marker = _markerRepository.GetMarkerById(id);

                JsonResult result = new(marker)
                {
                    StatusCode = 200
                };
                
                _loggerInfo.Info("SUCESSO AO ENCONTRAR MARCADOR POR ID");

                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on get the specific Marker " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("FALHA AO BUSCAR MARCADOR POR ID");
                return result;
            }
        }

        /// <summary>
        /// Get all markers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllMarkers()
        {
            try
            {
                IList<MarkerDTO> markers = _markerRepository.GetAllMarkers();

                JsonResult result = new(markers)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO ENCONTRAR MARCADOR");
                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on get all Themes: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("FALHA AO ENCONTRAR MARCADOR");
                return result;
            }
        }

        /// <summary>
        /// Create a new marker
        /// </summary>
        /// <param name="markerDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateMarker(MarkerDTO markerDTO)
        {
            try
            {
                MarkerDTO marker = _markerRepository.CreateMarker(markerDTO);

                JsonResult result = new(marker)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO CRIAR MARCADOR");
                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on create a new Marker: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO CRIAR MARCADOR");
                return result;
            }
        }

        /// <summary>
        /// Update a given marker
        /// </summary>
        /// <param name="markerDTO"></param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult UpdateMaker(UpdateMarkerDTO markerDTO)
        {
            try
            {
                MarkerDTO updatedMarker = _markerRepository.UpdateMarker(markerDTO);

                JsonResult result = new(updatedMarker)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO ATUALIZAR MARCADORES");
                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on update the given Marker: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO ATUALIZAR MARCADORES");
                return result;
            }
        }

        /// <summary>
        /// Switch a marker status by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult SwitchMarkerStatus(ulong id)
        {
            try
            {
                _markerRepository.SwitchStatusMarker(id);

                _loggerInfo.Info("SUCESSO AO ATUALIZAR STATUS DE MARCADORES");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on switch marker status from the given Maker: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO ATUALIZAR STATUS DE MARCADORES");
                return result;
            }
        }
    }
}
