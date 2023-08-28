namespace dosymep.SimpleServices {
    /// <summary>
    /// Фабрика создания сервиса прогресс диалога.
    /// </summary>
    public interface IProgressDialogFactory {
        /// <summary>
        /// Создает сервис прогресс диалога.
        /// </summary>
        /// <returns>Возвращает новый экземпляр сервиса прогресс диалога.</returns>
        IProgressDialogService CreateDialog();
    }
}