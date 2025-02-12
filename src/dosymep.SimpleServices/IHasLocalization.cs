using System.Globalization;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс с сервисами локализации.
    /// </summary>
    public interface IHasLocalization {
        /// <summary>
        /// Используемый язык.
        /// </summary>
        CultureInfo HostLanguage { get;  }
        
        /// <summary>
        /// Сервис языка.
        /// </summary>
        ILanguageService LanguageService { get; }
        
        /// <summary>
        /// Сервис локализации.
        /// </summary>
        ILocalizationService LocalizationService { get; }
    }
}