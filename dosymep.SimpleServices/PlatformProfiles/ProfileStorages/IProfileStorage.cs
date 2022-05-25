namespace dosymep.SimpleServices.PlatformProfiles.ProfileStorages {
    /// <summary>
    /// Интерфейс предоставляет доступ к профилям.
    /// </summary>
    public interface IProfileStorage {
        /// <summary>
        /// Возвращает и устанавливает используемое наименование профиля.
        /// </summary>
        string ProfileName { get; set; }
        
        /// <summary>
        /// Возвращает и устанавливает путь до локального хранения профилей.
        /// </summary>
        string ProfileLocalPath { get; set; }
        
        /// <summary>
        /// Возвращает информацию о сохраненных профилях в системе.
        /// </summary>
        /// <returns>Возвращает информацию о сохраненных профилях в системе.</returns>
        ProfileInfo[] GetProfileInfos();


        /// <summary>
        /// Возвращает информацию о текущем профиле.
        /// </summary>
        /// <returns>Возвращает информацию о текущем профиле.</returns>
        ProfileInfo GetCurrentProfileInfo();
        
        /// <summary>
        /// Возвращает определение профиля из хранилища.
        /// </summary>
        /// <param name="profileInfo">Информация профиля.</param>
        /// <param name="profileSpace">Пространство профиля.</param>
        /// <returns>Возвращает определение профиля из хранилища.</returns>
        IProfileInstance LoadProfileSpace(ProfileInfo profileInfo, ProfileSpace profileSpace);
    }
}