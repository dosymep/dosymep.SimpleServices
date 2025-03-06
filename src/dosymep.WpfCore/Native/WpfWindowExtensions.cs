using System.Windows;
using System.Windows.Interop;

namespace dosymep.WpfCore.Native;

internal static class WpfWindowExtensions {
    public static WINDOWPLACEMENT GetPlacement(this Window window) {
        return NativeMethods.GetPlacement(new WindowInteropHelper(window).Handle);
    }

    public static void SetPlacement(this Window window, WINDOWPLACEMENT placement) {
        NativeMethods.SetPlacement(new WindowInteropHelper(window).Handle, placement);
    }
}