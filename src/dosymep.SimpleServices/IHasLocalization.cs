using System;
using System.Globalization;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс с сервисами локализации.
    /// </summary>
    public interface IHasLocalization {
        /// <summary>
        /// Событие изменения языка.
        /// </summary>
        event Action<CultureInfo> LanguageChanged;
        
        /// <summary>
        /// Используемый язык.
        /// </summary>
        CultureInfo HostLanguage { get;  }
        
        /// <summary>
        /// Сервис локализации.
        /// </summary>
        ILocalizationService LocalizationService { get; }
    }
}