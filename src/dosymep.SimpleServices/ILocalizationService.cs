using System.Globalization;
using System.Windows;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Сервис локализации предоставляет простой интерфейс для получения локализированных строк.
    /// </summary>
    public interface ILocalizationService {
        /// <summary>
        /// Возвращает локализированную строку.
        /// </summary>
        /// <param name="name">Наименование локализированной строки.</param>
        /// <returns>Возвращает локализированную строку. Если строка не была найдена возвращает null.</returns>
        string GetLocalizedString(string name);

        /// <summary>
        /// Возвращает форматированную локализированную строку.
        /// </summary>
        /// <param name="name">Наименование локализированной строки.</param>
        /// <param name="args">Параметры форматирования локализированной строки.</param>
        /// <returns>Возвращает форматированную локализированную строку. Если строка не была найдена возвращает null.</returns>
        string GetLocalizedString(string name, params object[] args);

        /// <summary>
        /// Устанавливает ресурсы локализации для интерфейса.
        /// </summary>
        /// <param name="cultureInfo">Применяемые языковые стандарты.</param>
        void SetLocalization(CultureInfo cultureInfo);
        
        /// <summary>
        /// Устанавливает ресурсы локализации для интерфейса.
        /// </summary>
        /// <param name="cultureInfo">Применяемые языковые стандарты.</param>
        /// <param name="frameworkElement">Элемент интерфейса WPF.</param>
        void SetLocalization(CultureInfo cultureInfo, FrameworkElement frameworkElement);
    }
}