using dosymep.SimpleServices.PlatformProfiles;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс сервиса профилей.
    /// </summary>
    public interface IPlatformProfileService {
        /// <summary>
        /// Профиль пользователя.
        /// </summary>
        IProfileInstance User { get; }
        
        /// <summary>
        /// Профиль системы.
        /// </summary>
        IProfileInstance System { get; }
        
        /// <summary>
        /// Профиль организации 
        /// </summary>
        IProfileInstance Organization { get; }
    }
}