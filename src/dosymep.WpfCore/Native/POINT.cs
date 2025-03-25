// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace dosymep.WpfCore.Native;

/// <summary>
/// Структура POINT определяет координаты x и y точки.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
internal struct POINT {
    /// <summary>
    /// Указывает координату точки по оси X.
    /// </summary>
    public int x;
    
    /// <summary>
    /// Указывает координату точки по оси Y.
    /// </summary>
    public int y;
}