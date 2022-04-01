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
            if(ci.Name.Equals("en-US", StringComparison.CurrentCultureIgnoreCase))
                return Language.EnglishUSA;
            return Language.Russian;
        }
    }
}
