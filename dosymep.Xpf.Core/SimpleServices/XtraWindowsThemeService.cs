using System;
using DevExpress.Xpf.Core;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices
{
    public class XtraWindowsThemeService : IUIThemeService
    {
        public event Action<UIThemes> UIThemeChanged;
        
        private UIThemes _hostTheme = UIThemes.Light;
        
        public UIThemes HostTheme
        {
            get => _hostTheme;
            private set
            {
                _hostTheme = value;
                switch (_hostTheme)
                {
                    case UIThemes.Dark:
                        ApplicationThemeHelper.ApplicationThemeName = Theme.Win10DarkName;
                        break;
                    case UIThemes.Light:
                        ApplicationThemeHelper.ApplicationThemeName = Theme.Win10LightName;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }
    }
}