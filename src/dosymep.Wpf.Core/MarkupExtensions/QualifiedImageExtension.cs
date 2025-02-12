using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Xaml;

using dosymep.SimpleServices;

namespace dosymep.Wpf.Core.MarkupExtensions;

/// <summary>
/// Расширение выбора изображений, на основе темы окна и его языка.
/// </summary>
public sealed class QualifiedImageExtension : MarkupExtension {
    /// <summary>
    /// Конструирует объект.
    /// </summary>
    public QualifiedImageExtension() {
    }

    /// <summary>
    /// Путь до ресурса.
    /// </summary>
    public string? Uri { get; set; }

    /// <inheritdoc />
    public override object? ProvideValue(IServiceProvider serviceProvider) {
        if(Uri == null) {
            throw new InvalidOperationException("Uri cannot be null.");
        }

        IEnumerable<string> variations = GetVariations(serviceProvider);
        IUriContext uriContext = (IUriContext) serviceProvider.GetService(typeof(IUriContext));

        Uri baseUri = new(
            string.Concat("pack://application:,,,/", uriContext.BaseUri.Segments[1]), UriKind.Absolute);

        Uri[] items = variations
            .Select(item => new Uri(baseUri,
                new Uri(item, UriKind.Relative)))
            .Where(item => HasResources(item))
            .ToArray();

        if(items.Length > 1) {
            throw new InvalidOperationException("Find multiple images.");
        }

        if(items.Length == 1) {
            return BitmapFrame.Create(items[0]);
        }

        return default;
    }

    private IEnumerable<string> GetVariations(IServiceProvider serviceProvider) {
        IRootObjectProvider rootObjectProvider =
            (IRootObjectProvider) serviceProvider.GetService(typeof(IRootObjectProvider));

        IHasTheme? theme = rootObjectProvider.RootObject as IHasTheme;
        IHasLocalization? localization = rootObjectProvider.RootObject as IHasLocalization;

        string? themeName = theme?.HostTheme.ToString();
        string? cultureName = localization?.HostLanguage.Name;

        themeName = string.IsNullOrEmpty(themeName) ? null : $"theme-{themeName}";
        cultureName = string.IsNullOrEmpty(cultureName) ? null : $"lang-{cultureName}";

        string fileName = Path.GetFileName(Uri)!;
        string extension = Path.GetExtension(Uri)!;
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Uri)!;
        string? directoryName = Path.GetDirectoryName(Uri);

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

    private bool HasResources(Uri uri) {
        try {
            Application.GetResourceStream(uri);
            return true;
        } catch {
            return false;
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
                .Where((v, i) => (index & (1 << i)) != 0)
                .ToArray());
    }
}