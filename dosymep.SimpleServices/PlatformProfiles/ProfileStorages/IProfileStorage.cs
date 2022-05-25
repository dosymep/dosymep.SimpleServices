namespace dosymep.SimpleServices.PlatformProfiles.ProfileStorages {
    /// <summary>
    /// Интерфейс предоставляет доступ к профилям.
    /// </summary>
    internal interface IProfileStorage {
        /// <summary>
        /// Возвращает и устанавливает используемое наименование профиля.
        /// </summary>
        string ProfileName { get; set; }
        
        /// <summary>
        /// Возвращает информацию о сохраненных профилях в системе.
        /// </summary>
        /// <returns>Возвращает информацию о сохраненных профилях в системе.</returns>
        ProfileInfo[] GetProfileInfos();
        
        /// <summary>
        /// Возвращает определение профиля из хранилища.
        /// </summary>
        /// <param name="profileInfo">Информация профиля.</param>
        /// <param name="profileSpace">Пространство профиля.</param>
        /// <returns>Возвращает определение профиля из хранилища.</returns>
        IProfileInstance LoadProfileSpace(ProfileInfo profileInfo, ProfileSpace profileSpace);
    }
}