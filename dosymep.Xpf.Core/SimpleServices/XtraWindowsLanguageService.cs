using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Предоставляет доступ к языку Windows.
    /// </summary>
    public class XtraWindowsLanguageService : ILanguageService { 
        /// <inheritdoc/>
        public Language HostLanguage => GetWindowLanguage();

        private Language GetWindowLanguage() {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            switch(ci.Name) {
                case "en-US":
                return Language.EnglishUSA;
                case "ru-RU":
                return Language.Russian;
                case "de-DE":
                return Language.German;
                case "es-ES":
                return Language.Spanish;
                case "fr-FR":
                return Language.French;
                case "it-IT":
                return Language.Italian;
                case "nl-NL":
                return Language.Dutch;
                case "zh-CHS":
                return Language.ChineseSimplified;
                case "zh-CHT":
                return Language.ChineseTraditional;
                case "ja-JP":
                return Language.Japanese;
                case "ko-KR":
                return Language.Korean;
                case "cs-CZ":
                return Language.Czech;
                case "pl-PL":
                return Language.Polish;
                case "hu-HU":
                return Language.Hungarian;
                case "pt-BR":
                return Language.BrazilianPortuguese;
                case "en-GB":
                return Language.EnglishGB;
                default:
                return Language.EnglishUSA;
            }
        }
    }
}
