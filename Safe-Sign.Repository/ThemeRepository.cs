using Safe_Sign.DTO.Theme;
using Safe_Sign.DAL.Models;
using Safe_Sign.DAL.DAO.Base;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.Repository
{
    public class ThemeRepository : IThemeRepository
    {
        private readonly IDAO<Theme> _themeDAO;

        public ThemeRepository(IDAO<Theme> themeDAO)
        {
            _themeDAO = themeDAO;
        }

        public ThemeDTO GetThemeById(ulong idTheme)
        {
            Theme theme = _themeDAO.GetById(idTheme);

            if (theme == null) throw new NullReferenceException();

            else
            {
                ThemeDTO newDTO = new()
                {
                    Id = theme.Id,
                    Title = theme.Title
                };

                return newDTO;
            }
        }

        public IList<ThemeDTO> GetAllThemes()
        {
            IEnumerable<Theme> themes = _themeDAO.GetAll();

            if (themes is null) throw new NullReferenceException();

            else
            {
                List<ThemeDTO> themesDTO = new();

                foreach (Theme t in themes)
                {
                    ThemeDTO newDTO = new()
                    {
                        Id = t.Id,
                        Title = t.Title,
                    };

                    themesDTO.Add(newDTO);
                }

                return themesDTO;
            }
        }

        public ThemeDTO CreateTheme(ThemeDTO newTheme)
        {
            if (string.IsNullOrEmpty(newTheme.Title)) throw new NullReferenceException();

            else
            {
                Theme? registred = _themeDAO.GetAll().FirstOrDefault(t => t.Title.ToLower().Trim() == newTheme.Title.ToLower().Trim());
                
                if (registred == null)
                {
                    Theme newEntity = new()
                    {
                        Title = newTheme.Title
                    };

                    Theme theme = _themeDAO.Create(newEntity);

                    ThemeDTO newDTO = new()
                    {
                        Id = theme.Id,
                        Title = theme.Title
                    };

                    return newDTO;
                }

                else throw new Exception();
            }
        }

        public ThemeDTO UpdateTheme(UpdateThemeDTO theme)
        {
            Theme oldTheme = _themeDAO.GetById(theme.Id);

            if (oldTheme is null) throw new NullReferenceException();

            else
            {

                oldTheme.Title = !(oldTheme.Title.ToLower().Trim()).Equals(theme.Title.ToLower().Trim()) ? theme.Title : oldTheme.Title;

                _themeDAO.Update(oldTheme);

                ThemeDTO newTheme = new()
                {
                    Id = oldTheme.Id,
                    Title = oldTheme.Title
                };

                return newTheme;
            }
        }

        public void DeleteTheme(ulong idTheme)
        {
            Theme theme = _themeDAO.GetById(idTheme);

            if (theme == null) throw new NullReferenceException();

            else _themeDAO.Delete(theme);
        }
    }
}
