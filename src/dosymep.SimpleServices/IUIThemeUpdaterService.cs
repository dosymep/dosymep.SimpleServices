﻿using System;
using System.Windows;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Интерфейс по обновлению тем окон.
    /// </summary>
    public interface IUIThemeUpdaterService {
        /// <summary>
        /// Устанавливает текущую тему окна.
        /// </summary>
        /// <param name="window">Окно, которому устанавливают тему.</param>
        /// <param name="theme">Устанавливаемая тема окна.</param>
        [Obsolete("Рекомендуется использовать void UpdateTheme(FrameworkElement frameworkElement, UIThemes theme)")]
        void SetTheme(Window window, UIThemes theme);

        /// <summary>
        /// Устанавливает текущую тему окна.
        /// </summary>
        /// <param name="theme">Устанавливаемая тема окна.</param>
        /// <param name="frameworkElement">Элемент управления, которому устанавливают тему.</param>
        void SetTheme(UIThemes theme, FrameworkElement frameworkElement);
    }
}