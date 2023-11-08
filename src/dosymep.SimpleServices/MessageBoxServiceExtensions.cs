using System;
using System.Windows;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Расширения для сервиса сообщений.
    /// </summary>
    public static class MessageBoxServiceExtensions {
        /// <summary>
        /// Показывает сообщение с указанными параметрами.
        /// </summary>
        /// <param name="service">Сервис окна сообщений.</param>
        /// <param name="messageBoxText">Текст отображаемый в окне сообщения.</param>
        /// <returns>Возвращает кнопку, на которую кликнул пользователь.</returns>
        public static MessageBoxResult Show(this IMessageBoxService service, string messageBoxText) {
            return service.Show(messageBoxText, string.Empty);
        }

        /// <summary>
        /// Показывает сообщение с указанными параметрами.
        /// </summary>
        /// <param name="service">Сервис окна сообщений.</param>
        /// <param name="messageBoxText">Текст отображаемый в окне сообщения.</param>
        /// <param name="caption">Заголовок окна сообщения.</param>
        /// <returns>Возвращает кнопку, на которую кликнул пользователь.</returns>
        public static MessageBoxResult Show(this IMessageBoxService service, string messageBoxText, string caption) {
            return service.Show(messageBoxText, caption, MessageBoxButton.OK);
        }

        /// <summary>
        /// Показывает сообщение с указанными параметрами.
        /// </summary>
        /// <param name="service">Сервис окна сообщений.</param>
        /// <param name="messageBoxText">Текст отображаемый в окне сообщения.</param>
        /// <param name="caption">Заголовок окна сообщения.</param>
        /// <param name="button">Кнопки отображаемые в окне сообщения.</param>
        /// <returns>Возвращает кнопку, на которую кликнул пользователь.</returns>
        public static MessageBoxResult Show(
            this IMessageBoxService service, 
            string messageBoxText, string caption,
            MessageBoxButton button) {
            return service.Show(messageBoxText, caption, button, MessageBoxImage.None);
        }

        /// <summary>
        /// Показывает сообщение с указанными параметрами.
        /// </summary>
        /// <param name="service">Сервис окна сообщений.</param>
        /// <param name="messageBoxText">Текст отображаемый в окне сообщения.</param>
        /// <param name="caption">Заголовок окна сообщения.</param>
        /// <param name="button">Кнопки отображаемые в окне сообщения.</param>
        /// <param name="icon">отображаемая в окне сообщения.</param>
        /// <returns>Возвращает кнопку, на которую кликнул пользователь.</returns>
        public static MessageBoxResult Show(
            this IMessageBoxService service,
            string messageBoxText, string caption,
            MessageBoxButton button, MessageBoxImage icon) {
            return service.Show(messageBoxText, caption, button, icon, MessageBoxResult.None);
        }
    }
}