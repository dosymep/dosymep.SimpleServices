using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет доступ к языку интерфейса.
    /// </summary>
    public interface ILanguageService {

        /// <summary>
        /// Используемый язык.
        /// </summary>
        Language HostLanguage { get; }
    }

    /// <summary>
    /// Используемые языки.
    /// </summary>
    public enum Language {
        /// <summary>
        /// Английский.
        /// </summary>
        EnglishUSA,
        /// <summary>
        /// Русский.
        /// </summary>
        Russian
    }
}
