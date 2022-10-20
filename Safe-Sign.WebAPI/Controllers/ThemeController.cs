using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NLog;

using Safe_Sign.DTO.Theme;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(Roles = "0")]
    public class ThemeController : ControllerBase
    {
        private readonly IThemeRepository _themeRepository;
        private static readonly Logger _loggerInfo = LogManager.GetLogger("loggerInfoThemeFile");
        private static readonly Logger _loggerError = LogManager.GetLogger("loggerErrorThemeFile");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="themeRepository"></param>
        public ThemeController(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        /// <summary>
        /// Check for controller status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string HealthCheck() => "ThemeController is online, and working!";

        /// <summary>
        /// Get a theme by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetThemeById(ulong id)
        {
            try
            {
                ThemeDTO theme = _themeRepository.GetThemeById(id);

                JsonResult result = new(theme)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO OBTER TEMA POR ID");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on get the specific theme: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO OBTER TEMA POR ID");
                return result;
            }
        }

        /// <summary>
        /// Get all themes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllThemes()
        {
            try
            {
                IEnumerable<ThemeDTO> themes = _themeRepository.GetAllThemes();

                JsonResult result = new(themes)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO BUSCAR TODOS OS TEMAS");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on get all Themes: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO BUSCAR TODOS OS TEMAS");
                return result;
            }
        }

        /// <summary>
        /// Create a new theme for an marker
        /// </summary>
        /// <param name="themeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateTheme(ThemeDTO themeDTO)
        {
            try
            {
                ThemeDTO theme = _themeRepository.CreateTheme(themeDTO);

                JsonResult result = new(theme)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO CRIAR TEMA");
                return result;
            }
            catch (Exception ex)
            {
                JsonResult result = new("Error on create a new Theme: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO CRIAR TEMA");
                return result;
            }
        }

        /// <summary>
        /// Update a theme title
        /// </summary>
        /// <param name="themeDTO"></param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult UpdateTheme(UpdateThemeDTO themeDTO)
        {
            try
            {
                ThemeDTO theme = _themeRepository.UpdateTheme(themeDTO);

                JsonResult result = new(theme)
                {
                    StatusCode = 200
                };

                _loggerInfo.Info("SUCESSO AO ATUALIZAR TEMA");
                return result;
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on update the given Theme: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO ATUALIZAR TEMA");
                return result;
            }
        }

        /// <summary>
        /// Delete a theme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteTheme(ulong id)
        {
            try
            {
                _themeRepository.DeleteTheme(id);

                _loggerInfo.Info("SUCESSO AO DELETAR TEMA");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                // _log
                JsonResult result = new("Error on delete the giving Theme: " + ex.Message)
                {
                    StatusCode = 500
                };

                _loggerError.Error("ERRO AO DELETAR TEMA");
                return result;
            }
        }
    }
}
