using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ITService.UI.Extensions
{
    internal static class WindowExtensions
    {
        private const string User32 = "user32.dll";

        [DllImport(User32, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport(User32, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam);

        public static IntPtr SendMessage(this Window window, WM msg, IntPtr wParam, IntPtr lParam = default) =>
            SendMessage(window.GetWindowHandle(), msg, wParam, lParam);
        public static IntPtr SendMessage(this Window window, WM msg, SC wParam, IntPtr lParam = default)
        {
             return SendMessage(
                 hWnd: window.GetWindowHandle(),
                 msg: (uint) msg,
                 wParam: (IntPtr)wParam,
                 lParam: lParam == default ? (IntPtr)' ' : lParam);
        }

        public static IntPtr GetWindowHandle(this Window window)
        {
            var helper = new WindowInteropHelper(window);
            return helper.Handle;
        }

    }
}
