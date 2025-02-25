using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;
using dosymep.WpfUI.Core.SimpleServices;

using Wpf.Ui.Controls;

namespace dosymep.WpfUI.Core.Windows;

internal partial class WpfUIProgressWindow : IHasTheme, IHasLocalization, IDisposable {
    public static readonly DependencyProperty TitleTextProperty = DependencyProperty.Register(
        nameof(TitleText), typeof(string), typeof(WpfUIProgressWindow), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty WaitTextProperty = DependencyProperty.Register(
        nameof(WaitText), typeof(string), typeof(WpfUIProgressWindow), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty CancelButtonTextProperty = DependencyProperty.Register(
        nameof(CancelButtonText), typeof(string), typeof(WpfUIProgressWindow), new PropertyMetadata(default(string)));


    private static readonly string _progressWindowLanguage =
        "pack://application:,,,/dosymep.WpfUI.Core;component/assets/localizations/language.xaml";

    private readonly IHasTheme _theme;
    private readonly IHasLocalization _localization;
    private readonly ILocalizationService _internalLocalization;

    public event Action<UIThemes>? ThemeChanged;
    public event Action<CultureInfo>? LanguageChanged;

    private CancellationTokenSource? _cancellationTokenSource;


    /// <summary>
    /// Инициализирует окно прогресс бара.
    /// </summary>
    public WpfUIProgressWindow(
        IHasTheme theme,
        IHasLocalization localization) {
        _theme = theme;
        _localization = localization;

        _theme.ThemeChanged += _ => ThemeChanged?.Invoke(_);
        _localization.LanguageChanged += _ => LanguageChanged?.Invoke(_);

        _internalLocalization = new WpfLocalizationService(_progressWindowLanguage, _localization.HostLanguage);
        _internalLocalization.SetLocalization(_localization.HostLanguage, this);

        InitializeComponent();
    }

    public string TitleText {
        get => (string) GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    public string WaitText {
        get => (string) GetValue(WaitTextProperty);
        set => SetValue(WaitTextProperty, value);
    }

    public string CancelButtonText {
        get => (string) GetValue(CancelButtonTextProperty);
        set => SetValue(CancelButtonTextProperty, value);
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

    protected override void OnInitialized(EventArgs e) {
        base.OnInitialized(e);
        _internalLocalization.SetLocalization(_localization.HostLanguage);

        TitleText = GetLocalization("ProgressDialog.Title");
        WaitText = GetLocalization("ProgressDialog.PleaseWait");
        CancelButtonText = GetLocalization("ProgressDialog.CancelButton");
    }
    
    private void WpfUIProgressWindow_OnLoaded(object sender, RoutedEventArgs e) {
        RenderSize = new Size(MaxWidth, MaxHeight);
    }

    protected override void OnClosed(EventArgs e) {
        base.OnClosed(e);
        ResetOwnerWindowStyle();
        _cancellationTokenSource?.Cancel();
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
        WaitText = string.Format(
            DisplayTitleFormat ?? GetLocalization("ProgressDialog.PleaseWaitFormat"), currentValue, MaxValue);
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
        _cancelButton.IsEnabled = false;
        WaitText = GetLocalization("ProgressDialog.Canceling");
        _cancellationTokenSource?.Cancel();
    }

    private string GetLocalization(string localizationName) {
        return LocalizationService.GetLocalizedString(localizationName)
               ?? _internalLocalization.GetLocalizedString(localizationName);
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