// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace dosymep.WpfCore.Native;

/// <summary>
/// Структура RECT определяет прямоугольник по координатам верхнего левого и нижнего правого углов.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
internal struct RECT {
    /// <summary>
    /// Задает координату по оси X левого верхнего угла прямоугольника.
    /// </summary>
    public int left;
    
    /// <summary>
    /// Задает координату по оси Y левого верхнего угла прямоугольника.
    /// </summary>
    public int top;
    
    /// <summary>
    /// Задает координату X правого нижнего угла прямоугольника.
    /// </summary>
    public int right;
    
    /// <summary>
    /// Задает координату по оси Y правого нижнего угла прямоугольника.
    /// </summary>
    public int bottom;
}