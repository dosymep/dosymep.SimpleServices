using System.Globalization;

using dosymep.SimpleServices;

using Microsoft.Win32;

namespace dosymep.WpfCore.SimpleServices {
    /// <summary>
    /// Класс сервиса доступа к языку Windows.
    /// </summary>
    public class WpfWindowsLanguageService : ILanguageService, IDisposable {
        /// <inheritdoc/>
        public event Action<CultureInfo>? LanguageChanged;
        
        /// <summary>
        /// Конструирует объект.
        /// </summary>
        public WpfWindowsLanguageService() {
            SystemEvents.UserPreferenceChanged += OnSystemEventsOnUserPreferenceChanged;
        }

        /// <inheritdoc/>
        public CultureInfo HostLanguage => CultureInfo.InstalledUICulture;
        
        private void OnSystemEventsOnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) {
            if(e.Category == UserPreferenceCategory.General) {
                LanguageChanged?.Invoke(HostLanguage);
            }
        }

        /// <summary>
        /// Очищает подписку на событие.
        /// </summary>
        /// <param name="disposing">Указывает на очистку ресурсов.</param>
        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                SystemEvents.UserPreferenceChanged -= OnSystemEventsOnUserPreferenceChanged;
            }
        }

        /// <inheritdoc />
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
