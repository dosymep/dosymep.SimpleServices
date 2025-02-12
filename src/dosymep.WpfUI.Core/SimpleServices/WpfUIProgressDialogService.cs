using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

using dosymep.SimpleServices;
using dosymep.WpfUI.Core.Windows;

namespace dosymep.WpfUI.Core.SimpleServices;

/// <summary>
/// Класс сервиса прогресс диалога.
/// </summary>
public sealed class WpfUIProgressDialogService : WpfUIBaseService, IProgressDialogService {
    private readonly IUIThemeService _uiThemeService;
    private readonly IUIThemeUpdaterService _uiThemeUpdaterService;

    private WpfUIProgressWindow _wpfUIProgressWindow;
    private readonly WindowInteropHelper _windowInteropHelper;

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="languageService">Сервис языка.</param>
    /// <param name="localizationService">Сервис локализации.</param>
    /// <param name="uiThemeService">Сервис тем.</param>
    /// <param name="uiThemeUpdaterService">Сервис обновления тем.</param>
    public WpfUIProgressDialogService(
        ILanguageService languageService,
        ILocalizationService localizationService,
        IUIThemeService uiThemeService,
        IUIThemeUpdaterService uiThemeUpdaterService) {
        _uiThemeService = uiThemeService;
        _uiThemeUpdaterService = uiThemeUpdaterService;

        _wpfUIProgressWindow = new WpfUIProgressWindow(languageService, localizationService);
        _windowInteropHelper = new WindowInteropHelper(_wpfUIProgressWindow) {
            Owner = Process.GetCurrentProcess().MainWindowHandle
        };

        _uiThemeService.UIThemeChanged += SetTheme;
    }

    /// <inheritdoc />
    public void Dispose() {
        _uiThemeService.UIThemeChanged -= SetTheme;
        _wpfUIProgressWindow.Dispose();
    }

    /// <inheritdoc />
    public bool Indeterminate {
        get => _wpfUIProgressWindow.Indeterminate;
        set => _wpfUIProgressWindow.Indeterminate = value;
    }

    /// <inheritdoc />
    public int MaxValue {
        get => _wpfUIProgressWindow.MaxValue;
        set => _wpfUIProgressWindow.MaxValue = value;
    }

    /// <inheritdoc />
    public int StepValue {
        get => _wpfUIProgressWindow.StepValue;
        set => _wpfUIProgressWindow.StepValue = value;
    }

    /// <inheritdoc />
    public string? DisplayTitleFormat {
        get => _wpfUIProgressWindow.DisplayTitleFormat;
        set => _wpfUIProgressWindow.DisplayTitleFormat = value;
    }

    /// <inheritdoc />
    public IProgress<int> CreateProgress() {
        return _wpfUIProgressWindow.CreateProgress();
    }

    /// <inheritdoc />
    public IProgress<int> CreateAsyncProgress() {
        return _wpfUIProgressWindow.CreateAsyncProgress();
    }

    /// <inheritdoc />
    public CancellationToken CreateCancellationToken() {
        return _wpfUIProgressWindow.CreateCancellationToken();
    }

    /// <inheritdoc />
    public void Close() {
        _wpfUIProgressWindow.Close();
    }

    /// <inheritdoc />
    public void Show() {
        SetTheme(_uiThemeService.HostTheme);
        _wpfUIProgressWindow.Show();
    }

    /// <inheritdoc />
    public void ShowDialog() {
        SetTheme(_uiThemeService.HostTheme);
        _wpfUIProgressWindow.ShowDialog();
    }

    /// <inheritdoc />
    public override void Detach() {
        if(AllowAttach) {
            _wpfUIProgressWindow.ResetOwnerWindowStyle();
            _wpfUIProgressWindow.Owner = null;
        }
    }

    /// <inheritdoc />
    public override void Attach(DependencyObject dependencyObject) {
        if(AllowAttach) {
            AssociatedObject = dependencyObject;

            Window? window = GetWindow();
            if(window?.IsVisible == true) {
                _wpfUIProgressWindow.Owner = window;
                _wpfUIProgressWindow.SetOwnerWindowStyle();
            }
        }
    }

    private Window? GetWindow() {
        if(AssociatedObject == null) {
            return default;
        }

        return Window.GetWindow(AssociatedObject);
    }

    private void SetTheme(UIThemes theme) {
        _uiThemeUpdaterService.SetTheme(_wpfUIProgressWindow, theme);
    }
}