using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;
using dosymep.WpfUI.Core.Windows;

namespace dosymep.WpfUI.Core.SimpleServices;

/// <summary>
/// Класс сервиса прогресс диалога.
/// </summary>
public sealed class WpfUIProgressDialogService : WpfBaseService, IProgressDialogService {
    private WpfUIProgressWindow _wpfUIProgressWindow;
    private readonly WindowInteropHelper _windowInteropHelper;

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public WpfUIProgressDialogService(IHasTheme theme, IHasLocalization localization) {
        _wpfUIProgressWindow = new WpfUIProgressWindow(theme, localization);

        _windowInteropHelper = new WindowInteropHelper(_wpfUIProgressWindow) {
            Owner = Process.GetCurrentProcess().MainWindowHandle
        };
    }

    /// <inheritdoc />
    public void Dispose() {
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
        _wpfUIProgressWindow.Show();
    }

    /// <inheritdoc />
    public void ShowDialog() {
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
            
            SetAssociatedOwner(_wpfUIProgressWindow);
            _wpfUIProgressWindow.SetOwnerWindowStyle();
        }
    }
}