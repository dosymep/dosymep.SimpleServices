using System.Windows;

using dosymep.SimpleServices;

namespace dosymep.Xpf.Core.SimpleServices {
    /// <summary>
    /// Класс фабрики создания сервиса прогресс диалога.
    /// </summary>
    public sealed class ProgressDialogFactory : IProgressDialogFactory, IAttachableService {
        /// <summary>
        /// Сервис по получению тем.
        /// </summary>
        public IUIThemeService UIThemeService { get; set; }
        
        /// <summary>
        /// Сервис по установке тем.
        /// </summary>
        public IUIThemeUpdaterService UIThemeUpdaterService { get; set; }

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
        public string DisplayTitleFormat { get; set; }
        
        /// <inheritdoc />
        public IProgressDialogService CreateDialog() {
            var dialogService = new XtraProgressDialogService() {
                AllowAttach = true,
                UIThemeService = UIThemeService,
                UIThemeUpdaterService = UIThemeUpdaterService,
                StepValue = StepValue,
                Indeterminate = Indeterminate,
                DisplayTitleFormat = DisplayTitleFormat,
            };

            dialogService.Attach(AssociatedObject);
            return dialogService;
        }

        /// <inheritdoc />
        public bool IsAttached => AssociatedObject != null;
        
        /// <inheritdoc />
        public bool AllowAttach => true;
        
        /// <inheritdoc />
        public DependencyObject AssociatedObject { get; private set; }

        /// <inheritdoc />
        public void Detach() {
            AssociatedObject = null;
        }

        /// <inheritdoc />
        public void Attach(DependencyObject dependencyObject) {
            AssociatedObject = dependencyObject;
        }
    }
}