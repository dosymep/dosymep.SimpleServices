using System.Windows;

using dosymep.SimpleServices;
using dosymep.WpfCore.SimpleServices;

namespace dosymep.WpfUI.Core.SimpleServices;

/// <summary>
/// Класс фабрики создания сервиса прогресс диалога.
/// </summary>
public sealed class WpfUIProgressDialogFactory : WpfBaseService, IProgressDialogFactory {
    private readonly IHasTheme _theme;
    private readonly IHasLocalization _localization;

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public WpfUIProgressDialogFactory(IHasTheme theme, IHasLocalization localization) {
        _theme = theme;
        _localization = localization;
    }

    /// <summary>
    /// Шаг обновления значение прогресса.
    /// </summary>
    public int StepValue { get; set; }

    /// <summary>
    /// Указывает будет ли прогресс бар неопределенным
    /// </summary>
    public bool Indeterminate { get; set; }

    /// <summary>
    /// Строка форматирования отображения.
    /// </summary>
    public string? DisplayTitleFormat { get; set; }

    /// <inheritdoc />
    public IProgressDialogService CreateDialog() {
        WpfUIProgressDialogService dialogService =
            new(_theme, _localization) {
                StepValue = StepValue, 
                Indeterminate = Indeterminate, 
                DisplayTitleFormat = DisplayTitleFormat,
            };

        if(AssociatedObject != default) {
            dialogService.Attach(AssociatedObject);
        }

        return dialogService;
    }
}