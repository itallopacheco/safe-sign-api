using Safe_Sign.DTO.Theme;

namespace Safe_Sign.Repository.Interfaces
{
    public interface IThemeRepository
    {
        ThemeDTO GetThemeById(ulong idTheme);

        IList<ThemeDTO> GetAllThemes();

        ThemeDTO CreateTheme(ThemeDTO theme);

        ThemeDTO UpdateTheme(UpdateThemeDTO theme);

        void DeleteTheme(ulong idTheme);
    }
}
