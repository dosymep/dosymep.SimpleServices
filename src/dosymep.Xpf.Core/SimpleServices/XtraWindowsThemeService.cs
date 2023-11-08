﻿using System;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Internal;

using dosymep.SimpleServices;

using Microsoft.Win32;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса тем для окон.
    /// </summary>
    public class XtraWindowsThemeService : IUIThemeService, IDisposable {
        /// <inheritdoc />
        public event Action<UIThemes> UIThemeChanged;

        /// <summary>
        /// Создает экземпляр сервиса для окон.
        /// </summary>
        public XtraWindowsThemeService() {
            SystemEvents.UserPreferenceChanged += OnSystemEventsOnUserPreferenceChanged;
        }

        /// <inheritdoc />
        public UIThemes HostTheme => GetCurrentTheme();

        private void OnSystemEventsOnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) {
            if(e.Category == UserPreferenceCategory.General) {
                UIThemeChanged?.Invoke(HostTheme);
            }
        }

        private static UIThemes GetCurrentTheme() {
            RegistryKey registry =
                Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");

            int? isLight = (int?) registry?.GetValue("AppsUseLightTheme");
            if(isLight == null) {
                return UIThemes.Light;
            }

            return isLight == 1
                ? UIThemes.Light
                : UIThemes.Dark;
        }

        /// <summary>
        /// Очищает подписку на событие.
        /// </summary>
        /// <param name="disposing">Указывает на очистку ресурсов.</param>
        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                SystemEvents.UserPreferenceChanged -= OnSystemEventsOnUserPreferenceChanged;
            }
        }

        /// <inheritdoc />
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}