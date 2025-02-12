using System.Windows;

using dosymep.SimpleServices;

namespace dosymep.WpfUI.Core.SimpleServices;

/// <summary>
/// Класс фабрики создания сервиса прогресс диалога.
/// </summary>
public sealed class WpfUIProgressDialogFactory : WpfUIBaseService, IProgressDialogFactory {
    private readonly ILanguageService _languageService;
    private readonly ILocalizationService _localizationService;
    private readonly IUIThemeService _uiThemeService;
    private readonly IUIThemeUpdaterService _uiThemeUpdaterService;

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="languageService">Сервис языка.</param>
    /// <param name="localizationService">Сервис локализации.</param>
    /// <param name="uiThemeService">Сервис тем.</param>
    /// <param name="uiThemeUpdaterService">Сервис обновления тем.</param>
    public WpfUIProgressDialogFactory(
        ILanguageService languageService,
        ILocalizationService localizationService,
        IUIThemeService uiThemeService,
        IUIThemeUpdaterService uiThemeUpdaterService) {
        _languageService = languageService;
        _localizationService = localizationService;
        _uiThemeService = uiThemeService;
        _uiThemeUpdaterService = uiThemeUpdaterService;
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
            new(_languageService, _localizationService, _uiThemeService, _uiThemeUpdaterService) {
                StepValue = StepValue, Indeterminate = Indeterminate, DisplayTitleFormat = DisplayTitleFormat,
            };

        if(AssociatedObject != default) {
            dialogService.Attach(AssociatedObject);
        }

        return dialogService;
    }
}