/* ORIGINAL CREDITS
 * 
 * DeeShell - A shell replacement for Windows
 * Pravin Paratey (February 19, 2007)
 * 
 * Article: http://www.dustyant.com/articles/deeshell
 * 
 * Released under Creative Commons Attribution 2.5 Licence
 * http://creativecommons.org/licenses/by/2.5/
 */

/* Spar Focused Task Bar (SparTask) (SFTB)
 * 
 * Rebuild and modded by Spar
 * 
 * GNU General Public License v3.0
 */
using System;
using System.Text;
using System.Runtime.InteropServices;

namespace SparTask
{
    public class WinAPI
    {
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public enum WindowShowStyle : int
        {
            Hide = 0,
            ShowNormal = 1,
            ShowMinimized = 2,
            ShowMaximized = 3,
            Maximize = 3,
            ShowNormalNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActivate = 7,
            ShowNoActivate = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimized = 11
        }

        public enum SPI : int
        {
            SPI_SETWORKAREA = 0x002F,
            SPI_GETWORKAREA = 0x0030
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam,
            ref RECT pvParam, uint fWinIni);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hwnd, bool fUnknown);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr handle, IntPtr pid);

        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
    ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
    ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static string GetProcessPath(IntPtr hwnd)
        {
            try
            {
                uint pid = 0;
                GetWindowThreadProcessId(hwnd, out pid);
                System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById((int)pid);
                return proc.MainModule.FileName.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        public static System.Globalization.CultureInfo GetCurrentCulture()
        {
            var l = GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero));
            return new System.Globalization.CultureInfo((short)l.ToInt64());
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        public const int WM_COMMAND = 0x111;
        public const int MIN_ALL = 419;
        public const int MIN_ALL_UNDO = 416;

    }
}
