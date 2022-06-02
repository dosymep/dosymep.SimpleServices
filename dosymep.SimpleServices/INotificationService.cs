using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет методы для показа уведомлений.
    /// </summary>
    public interface INotificationService {
        /// <summary>
        /// Создает и возвращает предустановленное уведомление.
        /// </summary>
        /// <param name="title">Заголовок уведомления.</param>
        /// <param name="body">Текст отображаемый в уведомлении.</param>
        /// <param name="footer">Отображаемый текст снизу уведомления.</param>
        /// <param name="author">Автор плагина.</param>
        /// <param name="imageSource">Изображение отображаемое в уведомлении.</param>
        /// <returns>Возвращает созданное уведомление.</returns>
        INotification CreateNotification(string title, string body, string footer = null, string author = null, ImageSource imageSource = null);

        /// <summary>
        /// Создает и возвращает предустановленное уведомление предупреждения.
        /// </summary>
        /// <param name="title">Заголовок уведомления.</param>
        /// <param name="body">Текст отображаемый в уведомлении.</param>
        /// <param name="author">Автор плагина.</param>
        /// <param name="imageSource">Изображение отображаемое в уведомлении.</param>
        /// <returns>Возвращает созданное уведомление.</returns>
        INotification CreateFatalNotification(string title, string body, string author = null, ImageSource imageSource = null);

        /// <summary>
        /// Создает и возвращает предустановленное уведомление ошибки.
        /// </summary>
        /// <param name="title">Заголовок уведомления.</param>
        /// <param name="body">Текст отображаемый в уведомлении.</param>
        /// <param name="author">Автор плагина.</param>
        /// <param name="imageSource">Изображение отображаемое в уведомлении.</param>
        /// <returns>Возвращает созданное уведомление.</returns>
        INotification CreateWarningNotification(string title, string body, string author = null, ImageSource imageSource = null);
    }

    /// <summary>
    /// Интерфейс уведомления.
    /// </summary>
    public interface INotification {
        /// <summary>
        /// Скрыть уведомление.
        /// </summary>
        void Hide();
        
        /// <summary>
        /// Показать уведомление.
        /// </summary>
        /// <returns>Возвращает результат показа уведомления.</returns>
        Task<bool?> ShowAsync();
        
        /// <summary>
        /// Показать уведомление.
        /// </summary>
        /// <param name="millisecond">Время отображения уведомления, указывается в миллисекундах.</param>
        /// <returns>Возвращает результат показа уведомления.</returns>
        Task<bool?> ShowAsync(int millisecond);
        
        /// <summary>
        /// Показать уведомление.
        /// </summary>
        /// <param name="interval">Время отображения уведомления.</param>
        /// <returns>Возвращает результат показа уведомления.</returns>
        Task<bool?> ShowAsync(TimeSpan interval);
    }
}