namespace dosymep.SimpleServices
{
    public interface IUIThemeService
    {
        UIThemes HostTheme { get; }
        UIThemes WindowsUiTheme { get; }
        UIThemes CurrentUiTheme { get; } 
    }

    public enum UIThemes
    {
        Dark,
        Light,
    }
}