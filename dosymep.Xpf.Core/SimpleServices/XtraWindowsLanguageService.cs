using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс сервиса доступа к языку Windows.
    /// </summary>
    public class XtraWindowsLanguageService : ILanguageService {
        /// <inheritdoc/>
        public CultureInfo HostLanguage => CultureInfo.InstalledUICulture;
    }
}
