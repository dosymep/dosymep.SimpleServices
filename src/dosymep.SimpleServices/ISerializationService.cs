﻿namespace dosymep.SimpleServices {
    /// <summary>
    /// Сервис сериализации.
    /// </summary>
    public interface ISerializationService {
        /// <summary>
        /// Расширение файла при сериализации в файл.
        /// </summary>
        string FileExtension { get; }
        
        /// <summary>
        /// Сериализует объект в строку.
        /// </summary>
        /// <typeparam name="T">Тип объекта сериализации.</typeparam>
        /// <param name="object">Экземпляр объекта сериализации.</param>
        /// <returns>Возвращает строковое значение сериализованного объекта.</returns>
        string Serialize<T>(T @object);

        /// <summary>
        /// Десериализует строку в объект.
        /// </summary>
        /// <typeparam name="T">Тип результата десериализации.</typeparam>
        /// <param name="text">Строка десериализуемого объекта.</param>
        /// <returns>Возвращает экземпляр десериализованного объекта.</returns>
        T Deserialize<T>(string text);
    }
}