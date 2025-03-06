// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace dosymep.WpfCore.Native;

internal static class NativeMethods {
    /// <summary>
    /// Активирует и отображает окно. Если окно свернуто, развернуто или упорядочено,
    /// система восстанавливает его исходный размер и положение.
    /// Приложение должно указать этот флаг при первом отображении окна.
    /// </summary>
    private const int SW_SHOWNORMAL = 1;
    
    /// <summary>
    /// Активирует окно и отображает его как свернутое окно.
    /// </summary>
    private const int SW_SHOWMINIMIZED = 2;

    /// <summary>
    /// Извлекает состояние отображения и восстановленные, свернутые и развернутые позиции указанного окна.
    /// </summary>
    /// <param name="hWnd">Дескриптор окна.</param>
    /// <param name="lpwndpl">
    /// Указатель на структуру WINDOWPLACEMENT, которая получает сведения о состоянии отображения и положении.
    /// Перед вызовом GetWindowPlacement задайте для элемента длины значение sizeof(WINDOWPLACEMENT).
    /// GetWindowPlacement завершается ошибкой, если длинаlpwndpl> задана неправильно.
    /// </param>
    /// <returns>
    /// Если функция выполняется успешно, возвращается ненулевое значение.
    /// Если функция выполняется неудачно, возвращается нулевое значение. Дополнительные сведения об ошибке можно получить, вызвав GetLastError.
    /// </returns>
    /// <remarks>
    /// Элемент флагов WINDOWPLACEMENT, полученный этой функцией, всегда равен нулю.
    /// Если окно, определенное параметром hWnd , развернуто, элемент showCmd SW_SHOWMAXIMIZED.
    /// Если окно свернуто, параметр showCmd SW_SHOWMINIMIZED. В противном случае это SW_SHOWNORMAL.
    ///
    /// Элемент длины WINDOWPLACEMENT должен иметь значение sizeof(WINDOWPLACEMENT).
    /// Если этот элемент задан неправильно, функция возвращает значение FALSE.
    /// Дополнительные замечания о правильном использовании координат размещения окна см. в разделе WINDOWPLACEMENT.
    /// </remarks>
    [DllImport("user32.dll")]
    private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hWnd">Дескриптор окна.</param>
    /// <param name="lpwndpl">
    /// Указатель на структуру WINDOWPLACEMENT, которая задает новое состояние отображения и позиции окна.
    /// Перед вызовом SetWindowPlacement задайте для элемента длины структуры WINDOWPLACEMENT значение sizeof(WINDOWPLACEMENT).
    /// SetWindowPlacement завершается ошибкой, если член длины задан неправильно.
    /// </param>
    /// <returns>
    /// Если функция выполняется успешно, возвращается ненулевое значение.
    /// Если функция выполняется неудачно, возвращается нулевое значение. Дополнительные сведения об ошибке можно получить, вызвав GetLastError.
    /// </returns>
    /// <remarks>
    /// Если информация, указанная в windowplacement, приведет к тому, что окно полностью отключается от экрана,
    /// система автоматически корректирует координаты таким образом, чтобы окно было видимым,
    /// учитывая изменения в разрешении экрана и конфигурации нескольких мониторов.
    ///
    /// Элемент длины WINDOWPLACEMENT должен иметь значение sizeof(WINDOWPLACEMENT).
    /// Если этот элемент задан неправильно, функция возвращает значение FALSE.
    /// Дополнительные замечания о правильном использовании координат размещения окна см. в разделе WINDOWPLACEMENT.
    /// </remarks>
    [DllImport("user32.dll")]
    private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);
    
    public static WINDOWPLACEMENT GetPlacement(IntPtr windowHandle) {
        GetWindowPlacement(windowHandle, out WINDOWPLACEMENT placement);
        return placement;
    }

    public static void SetPlacement(IntPtr windowHandle, WINDOWPLACEMENT placement) {
        placement.flags = 0;
        placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
        
        // восстанавливаем окно всегда развернутым
        placement.showCmd = placement.showCmd == SW_SHOWMINIMIZED ? SW_SHOWNORMAL : placement.showCmd;
        
        SetWindowPlacement(windowHandle, ref placement);
    }
}