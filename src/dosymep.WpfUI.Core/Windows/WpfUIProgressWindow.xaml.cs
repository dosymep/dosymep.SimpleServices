using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;

using dosymep.SimpleServices;
using dosymep.WpfUI.Core.SimpleServices;

namespace dosymep.WpfUI.Core.Windows;

internal partial class WpfUIProgressWindow : IHasTheme, IHasLocalization, IDisposable {
    private readonly IHasTheme _theme;
    private readonly IHasLocalization _localization;
    
    public event Action<UIThemes>? ThemeChanged;
    public event Action<CultureInfo>? LanguageChanged;
    
    private CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    /// Инициализирует окно прогресс бара.
    /// </summary>
    public WpfUIProgressWindow(
        IHasTheme theme,
        IHasLocalization localization) {
        InitializeComponent();
        
        _theme = theme;
        _localization = localization;

        _theme.ThemeChanged += ThemeChanged;
        _localization.LanguageChanged += LanguageChanged;
    }

    public UIThemes HostTheme => _theme.HostTheme;
    public IUIThemeUpdaterService ThemeUpdaterService => _theme.ThemeUpdaterService;
    public CultureInfo HostLanguage => _localization.HostLanguage;
    public ILocalizationService LocalizationService => _localization.LocalizationService;

    public bool Indeterminate {
        get => _progressEdit.IsIndeterminate;
        set => _progressEdit.IsIndeterminate = value;
    }

    public int MaxValue { get; set; }

    public int StepValue { get; set; }

    public string? DisplayTitleFormat { get; set; }

    public IProgress<int> CreateProgress() {
        return new CustomProgress(this);
    }

    public IProgress<int> CreateAsyncProgress() {
        return new Progress<int>(UpdateWindow);
    }

    /// <summary>
    /// Возвращает токен отмены.
    /// </summary>
    /// <returns>Возвращает токен отмены.</returns>
    public CancellationToken CreateCancellationToken() {
        if(_cancellationTokenSource == null) {
            _cancelButton.Visibility = Visibility.Visible;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        return _cancellationTokenSource.Token;
    }

    internal void SetOwnerWindowStyle() {
        if(Owner != null) {
            Owner.IsEnabled = false;
        }
    }

    internal void ResetOwnerWindowStyle() {
        if(Owner != null) {
            Owner.IsEnabled = true;
        }
    }

    /// <summary>
    /// Обновляет окно.
    /// </summary>
    internal void DispatcherUpdateWindow(int currentValue) {
        Dispatcher.Invoke(() => UpdateWindow(currentValue), DispatcherPriority.Background);
    }

    /// <summary>
    /// Обновляет окно.
    /// </summary>
    internal void UpdateWindow(int currentValue) {
        ++currentValue;
        if(StepValue > 0 && currentValue < MaxValue) {
            if(currentValue % StepValue == 0) {
                UpdateWindowImpl(currentValue);
            }
        } else {
            UpdateWindowImpl(currentValue);
        }
    }

    private void UpdateWindowImpl(int currentValue) {
        _progressEdit.Maximum = MaxValue;
        _progressEdit.Value = currentValue;
        _textEdit.Text = string.Format(
            DisplayTitleFormat ?? LocalizationService.GetLocalizedString("ProgressBar.PleaseWaitFormat"), currentValue, MaxValue);
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
        _cancelButton.IsEnabled = false;
        _textEdit.Text = LocalizationService.GetLocalizedString("ProgressBar.Canceling");
        _cancellationTokenSource?.Cancel();
    }

    #region IDispose

    ~WpfUIProgressWindow() {
        Dispose(false);
    }

    /// <inheritdoc />
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Очищает подписку на событие.
    /// </summary>
    /// <param name="disposing">Указывает на очистку ресурсов.</param>
    protected virtual void Dispose(bool disposing) {
        if(disposing) {
            Close();
            _theme.ThemeChanged -= ThemeChanged;
            _localization.LanguageChanged -= LanguageChanged;
        }
    }

    #endregion
}

internal class CustomProgress : IProgress<int> {
    private readonly WpfUIProgressWindow _window;

    public CustomProgress(WpfUIProgressWindow window) {
        _window = window;
    }

    public void Report(int value) {
        _window.DispatcherUpdateWindow(value);
    }
}