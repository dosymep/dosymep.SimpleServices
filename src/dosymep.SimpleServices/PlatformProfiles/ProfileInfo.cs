namespace dosymep.SimpleServices.PlatformProfiles {
    /// <summary>
    /// Класс предоставляет информацию о профиле.
    /// </summary>
    public class ProfileInfo {
        /// <summary>
        /// Возвращает и устанавливает значение профиля.
        /// </summary>
        internal string Name { get; set; }
        
        /// <summary>
        /// Возвращает и устанавливает полное имя профиля.
        /// </summary>
        internal string FullName { get; set; }
        
        /// <summary>
        /// Возвращает и устанавливает свойство только чтение.
        /// </summary>
        internal bool IsReadOnly { get; set; }
    }
}