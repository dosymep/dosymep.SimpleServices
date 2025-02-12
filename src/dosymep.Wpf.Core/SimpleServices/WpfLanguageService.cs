using System.Globalization;

using dosymep.SimpleServices;

namespace dosymep.Wpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса доступа к языку Windows.
    /// </summary>
    public class WpfLanguageService : ILanguageService {
        /// <inheritdoc/>
        public CultureInfo HostLanguage => CultureInfo.InstalledUICulture;
    }
}
