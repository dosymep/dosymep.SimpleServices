﻿using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса локализации.
    /// </summary>
    public sealed class XtraLocalizationService : ILocalizationService {
        private readonly string _resourceName;
        private readonly ResourceDictionary _defaultDictionary;

        private ResourceDictionary _dictionary = new ResourceDictionary();

        /// <summary>
        /// Создает экземпляр сервиса локализации.
        /// </summary>
        /// <param name="resourceName">Отночительный или абсолютный путь до ресурсов <see cref="ResourceDictionary"/> локализации.</param>
        /// <param name="defaultCulture">Языковые стандарты по умолчанию. Применяются, если не были найдены основные.</param>
        /// <exception cref="ArgumentException"><paramref name="resourceName"/> пустое или null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="defaultCulture"/> если null.</exception>
        public XtraLocalizationService(string resourceName, CultureInfo defaultCulture) {
            if(string.IsNullOrEmpty(resourceName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(resourceName));
            }

            if(defaultCulture == null) {
                throw new ArgumentNullException(nameof(defaultCulture));
            }

            _resourceName = resourceName;
            _defaultDictionary = CreateResourceDictionary(_resourceName, defaultCulture) ?? new ResourceDictionary();
        }

        /// <inheritdoc />
        public void SetLocalization(CultureInfo cultureInfo, FrameworkElement frameworkElement) {
            if(cultureInfo == null) {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            if(frameworkElement == null) {
                throw new ArgumentNullException(nameof(frameworkElement));
            }

            frameworkElement.Resources.MergedDictionaries.Remove(_dictionary);

            _dictionary = CreateResourceDictionary(_resourceName, cultureInfo)
                          ?? CreateResourceDictionary(_resourceName, default) ?? new ResourceDictionary();

            if(_dictionary.Source != _defaultDictionary.Source) {
                _dictionary.MergedDictionaries.Add(_defaultDictionary);
            }

            frameworkElement.Resources.MergedDictionaries.Add(_dictionary);
        }

        /// <inheritdoc />
        public string GetLocalizedString(string name) {
            if(string.IsNullOrEmpty(name)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            return _dictionary[name] as string;
        }

        /// <inheritdoc />
        public string GetLocalizedString(string name, params object[] args) {
            string format = GetLocalizedString(name);
            return format is null ? null : string.Format(format, args);
        }

        private string GetResourceName(string resourceName, CultureInfo cultureInfo) {
            var extension = Path.GetExtension(resourceName);
            return Path.ChangeExtension(resourceName, $"{cultureInfo.Name}{extension}");
        }

        private ResourceDictionary CreateResourceDictionary(string resourceName, CultureInfo cultureInfo) {
            if(cultureInfo == null) {
                return new ResourceDictionary() {Source = new Uri(resourceName, UriKind.RelativeOrAbsolute)};
            }

            try {
                return new ResourceDictionary() {
                    Source = new Uri(GetResourceName(resourceName, cultureInfo), UriKind.RelativeOrAbsolute)
                };
            } catch(IOException) {
                return default;
            } catch(WebException) {
                return default;
            }
        }
    }
}