using System;
using System.Collections.Generic;
using System.Globalization;
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
        CultureInfo HostLanguage { get; }
    }
}