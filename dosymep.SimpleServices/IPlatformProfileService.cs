using System;

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
    
    /// <summary>
    /// Интерфейс предоставляет доступ к настройкам плагинов.
    /// </summary>
    /// <typeparam name="TSpaceProfile">Класс пространства конфигурации.</typeparam>
    public interface IPlatformProfileService<TSpaceProfile> : IPlatformProfileService
        where TSpaceProfile : ProfileSpace {
        /// <summary>
        /// Возвращает экземпляр настроек плагина.
        /// </summary>
        /// <param name="pluginName">Наименование плагина.</param>
        /// <typeparam name="T">Тип настроек плагина.</typeparam>
        /// <returns>Возвращает экземпляр настроек плагина.</returns>
        T GetPluginSettings<T>(string pluginName);

        /// <summary>
        /// Возвращает экземпляр настроек плагина.
        /// </summary>
        /// <param name="pluginName">Наименование плагина.</param>
        /// <param name="settingsName">Наименование настроек.</param>
        /// <typeparam name="T">Тип настроек плагина.</typeparam>
        /// <returns>Возвращает экземпляр настроек плагина.</returns>
        T GetPluginSettings<T>(string pluginName, string settingsName);

        /// <summary>
        /// Сохраняет настройки плагина.
        /// </summary>
        /// <param name="settings">Настройки плагина.</param>
        /// <param name="pluginName">Наименование плагина.</param>
        /// <typeparam name="T">Тип настроек плагина.</typeparam>
        void SavePluginSettings<T>(T settings, string pluginName);

        /// <summary>
        /// Сохраняет настройки плагина.
        /// </summary>
        /// <param name="settings">Настройки плагина.</param>
        /// <param name="pluginName">Наименование плагина.</param>
        /// <param name="settingsName">Наименование настроек.</param>
        /// <typeparam name="T">Тип настроек плагина.</typeparam>
        void SavePluginSettings<T>(T settings, string pluginName, string settingsName);
    }

    /// <summary>
    /// Абстрактный класс настроек пространства профиля.
    /// </summary>
    public abstract class ProfileSpace : IEquatable<ProfileSpace> {
        /// <summary>
        /// Создает экземпляр пространства настроек профиля.
        /// </summary>
        /// <param name="name"></param>
        protected ProfileSpace(string name) {
            Name = name;
        }

        /// <summary>
        /// Пользовательские настройки пространства профиля.
        /// </summary>
        public static UserSpace UserSpace => new UserSpace("User");

        /// <summary>
        /// Системные настройки пространства профиля.
        /// </summary>
        public static SystemSpace SystemSpace => new SystemSpace("System");

        /// <summary>
        /// Организации настройки пространства профиля.
        /// </summary>
        public static OrganizationSpace OrganizationSpace => new OrganizationSpace("Organization");

        /// <summary>
        /// Наименование пространства профиля.
        /// </summary>
        public string Name { get; }

        #region IEquatable

        /// <inheritdoc />
        public bool Equals(ProfileSpace other) {
            if(ReferenceEquals(null, other)) {
                return false;
            }

            if(ReferenceEquals(this, other)) {
                return true;
            }

            return string.Equals(Name, other.Name, StringComparison.CurrentCulture);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj)) {
                return false;
            }

            if(ReferenceEquals(this, obj)) {
                return true;
            }

            if(obj.GetType() != this.GetType()) {
                return false;
            }

            return Equals((ProfileSpace) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return (Name != null ? StringComparer.CurrentCulture.GetHashCode(Name) : 0);
        }

        /// <summary>
        /// Оператор сравнения.
        /// </summary>
        /// <param name="left">Левый операнд.</param>
        /// <param name="right">Правый операнд.</param>
        /// <returns>Возвращает true - если левый операнд равен правому, иначе false.</returns>
        public static bool operator ==(ProfileSpace left, ProfileSpace right) {
            return Equals(left, right);
        }

        /// <summary>
        /// Оператор сравнения.
        /// </summary>
        /// <param name="left">Левый операнд.</param>
        /// <param name="right">Правый операнд.</param>
        /// <returns>Возвращает true - если левый операнд не равен правому, иначе false.</returns>
        public static bool operator !=(ProfileSpace left, ProfileSpace right) {
            return !Equals(left, right);
        }

        #endregion
    }

    /// <summary>
    /// Пользовательские настройки пространства профиля.
    /// </summary>
    public sealed class UserSpace : ProfileSpace {
        /// <summary>
        /// Создает экземпляр пользовательского пространства настроек профиля.
        /// </summary>
        /// <param name="name">Наименование профиля.</param>
        internal UserSpace(string name)
            : base(name) {
        }
    }

    /// <summary>
    /// Системные настройки пространства профиля.
    /// </summary>
    public sealed class SystemSpace : ProfileSpace {
        /// <summary>
        /// Создает экземпляр системного пространства настроек профиля.
        /// </summary>
        /// <param name="name">Наименование профиля.</param>
        internal SystemSpace(string name)
            : base(name) {
        }
    }

    /// <summary>
    /// Организации настройки пространства профиля.
    /// </summary>
    public sealed class OrganizationSpace : ProfileSpace {
        /// <summary>
        /// Создает экземпляр организации пространства настроек профиля.
        /// </summary>
        /// <param name="name">Наименование профиля.</param>
        internal OrganizationSpace(string name)
            : base(name) {
        }
    }
}