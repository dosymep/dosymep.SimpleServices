﻿namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет методы и свойства для конфигурации стандартного диалога.
    /// </summary>
    public interface IFileDialogServiceBase {
        /// <summary>
        /// Возвращает или устанавливает значение признака автоматической установки расширения к имени файла.
        /// </summary>
        bool AddExtension { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает значение признака автоматической смены внешнего вида и поведения.
        /// </summary>
        bool AutoUpgradeEnabled { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает признак отображения предупреждения, когда пользователь указывает не существующее имя файла.
        /// </summary>
        bool CheckFileExists { get; set; }

        /// <summary>
        /// Возвращает или устанавливает признак отображения предупреждения, когда пользователь указывает не существующий путь.
        /// </summary>
        bool CheckPathExists { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает признак принятия только допустимых имен файлов Win32.
        /// </summary>
        bool ValidateNames { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает признак возвращения расположения файла, на который ссылается ярлык или возвращает расположения ярлыка.
        /// </summary>
        bool DereferenceLinks { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает признак запоминания предыдущего каталога при повторном отображении. 
        /// </summary>
        bool RestoreDirectory { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает признак отображения кнопки "Справка".
        /// </summary>
        bool ShowHelp { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает признак отображения и сохранения файлов с несколькими расширениями.
        /// </summary>
        bool SupportMultiDottedExtensions { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает индекс параметра фильтрации.
        /// </summary>
        int FilterIndex { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает отображаемый заголовок.
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает строку фильтра, указывающую на параметры доступные в поле "Тип файлов".
        /// </summary>
        string Filter { get; set; }
        
        /// <summary>
        /// Возвращает или устанавливает исходную папку. 
        /// </summary>
        string InitialDirectory { get; set; }
        
        /// <summary>
        /// Сбрасывает все свойства до значений по умолчанию.
        /// </summary>
        void Reset();
    }
}