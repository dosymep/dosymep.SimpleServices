using System.Globalization;

using dosymep.SimpleServices;

namespace dosymep.WpfCore.SimpleServices {
    /// <summary>
    /// Класс сервиса доступа к языку Windows.
    /// </summary>
    public class WpfWindowsLanguageService : ILanguageService {
        /// <inheritdoc/>
        public CultureInfo HostLanguage => CultureInfo.InstalledUICulture;
    }
}
