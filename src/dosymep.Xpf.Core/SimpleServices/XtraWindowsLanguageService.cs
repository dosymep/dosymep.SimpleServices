using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dosymep.SimpleServices;

using Microsoft.Win32;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса доступа к языку Windows.
    /// </summary>
    public class XtraWindowsLanguageService : ILanguageService, IDisposable {
        /// <inheritdoc />
        public event Action<CultureInfo> LanguageChanged;

        /// <inheritdoc/>
        public CultureInfo HostLanguage => CultureInfo.InstalledUICulture;

        /// <summary>
        /// Конструирует объект.
        /// </summary>
        public XtraWindowsLanguageService() {
            SystemEvents.UserPreferenceChanged += OnSystemEventsOnUserPreferenceChanged;
        }

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
