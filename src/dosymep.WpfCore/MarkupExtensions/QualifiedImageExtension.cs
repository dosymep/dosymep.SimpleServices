using System.Collections;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

using dosymep.SimpleServices;
using dosymep.WpfCore.Converters;
using dosymep.WpfCore.MarkupExtensions.Internal;

namespace dosymep.WpfCore.MarkupExtensions;

/// <summary>
/// Расширение выбора изображений, на основе темы окна и его языка.
/// </summary>
public sealed class QualifiedImageExtension : MarkupExtension {
    private readonly MarkupValueObject _markupValueObject = new();

    private readonly Binding _binding = new(nameof(MarkupValueObject.Value)) {
        Converter = new TypeConverterDecorator(new ImageSourceConverter())
    };

    private Uri? _cacheBaseUri;
    private string? _cacheLibName;
    private HashSet<string>? _cacheResources;

    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public QualifiedImageExtension() { }


    /// <summary>
    /// Конструирует объект.
    /// </summary>
    /// <param name="uri">Путь до ресурса.</param>
    public QualifiedImageExtension(string? uri) => Uri = uri;

    /// <summary>
    /// Путь до ресурса.
    /// </summary>
    public string? Uri { get; set; }

    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider) {
        if(Uri == null) {
            throw new InvalidOperationException("Uri is not set.");
        }

        // получаем название библиотеки
        _cacheLibName = GetLibName(serviceProvider);
        _cacheBaseUri = new Uri($"pack://application:,,,/{_cacheLibName};component/");
        _cacheResources ??= GetResources(_cacheLibName).ToHashSet(StringComparer.OrdinalIgnoreCase);

        // получаем корневой элемент,
        // может быть Window, Page, UserControl
        FrameworkElement? rootObject = serviceProvider.GetRootObject<FrameworkElement>();
        if(rootObject?.IsLoaded == true) {
            // если элемент был загружен, устанавливаем значения
            IHasTheme? theme = serviceProvider.GetRootObject<IHasTheme>();
            IHasLocalization? localization = serviceProvider.GetRootObject<IHasLocalization>();

            UpdateImage(theme, localization);
        } else {
            if(rootObject != null) {
                // либо ждем его загрузку,
                // чтобы можно было получить корневой элемент Window
                rootObject.Loaded += RootObjectOnLoaded;
            }
        }

        _binding.Source = _markupValueObject;
        return _binding.ProvideValue(serviceProvider);
    }

    private void RootObjectOnLoaded(object sender, RoutedEventArgs e) {
        if(sender is not FrameworkElement frameworkElement) {
            return;
        }

        // Отписываемся от события,
        // потому что при смене темы может повторно вызваться
        frameworkElement.Loaded -= RootObjectOnLoaded;

        Window? rootWindow = Window.GetWindow(frameworkElement);
        UpdateImage(rootWindow as IHasTheme, rootWindow as IHasLocalization);
    }

    private void UpdateImage(IHasTheme? theme, IHasLocalization? localization) {
        if(theme is not null) {
            theme.ThemeChanged += _ => _markupValueObject.Value = GetImageUri(theme, localization);
        }

        if(localization is not null) {
            localization.LanguageChanged += _ => _markupValueObject.Value = GetImageUri(theme, localization);
        }

        _markupValueObject.Value = GetImageUri(theme, localization);
    }

    private Uri GetImageUri(IHasTheme? theme, IHasLocalization? localization) {
        HashSet<string> variations = GetVariations(theme, localization)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach(string resourceName in _cacheResources!) {
            if(variations!.Contains(resourceName)) {
                return new Uri(_cacheBaseUri!, resourceName);
            }
        }

        // return no image
        return new Uri(
            "pack://application:,,,/dosymep.Wpf.Core;component/assets/images/icons8-no-image-96.png", UriKind.Absolute);
    }

    private string GetLibName(IServiceProvider serviceProvider) {
        IUriContext uriContext = (IUriContext) serviceProvider.GetService(typeof(IUriContext));

        string? libName = Uri?.Split('/')
            .FirstOrDefault(item => item.EndsWith(";component"));

        libName ??= uriContext.BaseUri.Segments[1].Trim('/');
        return libName.Replace(";component", string.Empty);
    }

    private IEnumerable<string> GetVariations(IHasTheme? theme, IHasLocalization? localization) {
        string? themeName = theme?.HostTheme.ToString();
        string? cultureName = localization?.HostLanguage.Name;

        themeName = string.IsNullOrEmpty(themeName) ? null : $"theme-{themeName}";
        cultureName = string.IsNullOrEmpty(cultureName) ? null : $"lang-{cultureName}";

        string fileName = Path.GetFileName(Uri)!;
        string extension = Path.GetExtension(Uri)!;
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Uri)!;
        string? directoryName = Uri
            ?.Substring(0, Uri.LastIndexOf('/'))
            .Replace($"{_cacheLibName};component", string.Empty)
            .Trim('/');

        IEnumerable<string[]> list = Combinations(
            new[] {themeName, cultureName}
                .Where(item => !string.IsNullOrEmpty(item))
                .OfType<string>());

        foreach(string[] item in list) {
            string val1 = string.Join("/", item);
            string val2 = string.Join("_", item);

            if(!string.IsNullOrEmpty(val1)) {
                yield return $"{directoryName}/{val1}/{fileName}";
            }

            if(!string.IsNullOrEmpty(val2)) {
                yield return $"{directoryName}/{fileNameWithoutExtension}.{val2}{extension}";
            }
        }

        if(!string.IsNullOrEmpty(themeName)
           && !string.IsNullOrEmpty(cultureName)) {
            yield return $"{directoryName}/{themeName}/{fileNameWithoutExtension}.{cultureName}{extension}";
            yield return $"{directoryName}/{cultureName}/{fileNameWithoutExtension}.{themeName}{extension}";
        }

        yield return string.Join("/", directoryName, fileName);
    }

    private List<string> GetResources(string libName) {
        List<string> resources = [];

        try {
            Assembly? assembly = GetAssembly(libName);
            using Stream? stream = assembly?.GetManifestResourceStream(libName + ".g.resources");

            using ResourceReader resourceReader = new(stream!);
            resources.AddRange(from DictionaryEntry resource in resourceReader select (string) resource.Key);
        } catch {
            // pass
        }

        return resources;
    }

    private Assembly? GetAssembly(string libName) {
        try {
            return Assembly.Load(libName);
        } catch {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .LastOrDefault(item => item.GetName().Name.Equals(_cacheLibName));
        }
    }

    private static IEnumerable<T[]> Combinations<T>(IEnumerable<T> source) {
        if(source == null) {
            throw new ArgumentNullException(nameof(source));
        }

        T[] data = source.ToArray();

        return Enumerable
            .Range(0, 1 << (data.Length))
            .Select(index => data
                .Where((_, i) => (index & (1 << i)) != 0)
                .ToArray());
    }
}