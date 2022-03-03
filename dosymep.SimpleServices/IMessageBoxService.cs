using System.Windows;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет методы для показа окна сообщений.
    /// </summary>
    public interface IMessageBoxService {
        /// <summary>
        /// Показывает сообщение с указанными параметрами.
        /// </summary>
        /// <param name="messageBoxText">Текст отображаемый в окне сообщения.</param>
        /// <param name="caption">Заголовок окна сообщения.</param>
        /// <param name="button">Кнопки отображаемые в окне сообщения.</param>
        /// <param name="icon">Иконка отображаемая в окне сообщения.</param>
        /// <param name="defaultResult">Выбираемая кнопка по умолчанию при показе окна сообщения.</param>
        /// <returns>Возвращает кнопку, на которую кликнул пользователь.</returns>
        MessageBoxResult Show(string messageBoxText, string caption,
            MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult);
    }
}