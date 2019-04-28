/* DeeShell - A shell replacement for Windows
 * Pravin Paratey (February 19, 2007)
 * 
 * Article: http://www.dustyant.com/articles/
 * 
 * Released under Creative Commons Attribution 2.5 Licence
 * http://creativecommons.org/licenses/by/2.5/
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

        /// <summary>For ShowWindow</summary>
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

        /// <summary>For SystemParametersInfo</summary>
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
        public static extern void SwitchToThisWindow(IntPtr hwnd,bool fUnknown);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
            int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

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
                System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById((int)pid); //Gets the process by ID. 
                return proc.MainModule.FileName.ToString();    //Returns the path. 
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [DllImport("user32.dll", EntryPoint = "FindWindowEx",CharSet = CharSet.Auto)]
            static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public static System.Collections.ArrayList GetAllChildrenWindowHandles(IntPtr hParent,int maxCount)
        {
            System.Collections.ArrayList result = new System.Collections.ArrayList();
            int ct = 0;
            IntPtr prevChild = IntPtr.Zero;
            IntPtr currChild = IntPtr.Zero;
            while (true && ct < maxCount)
            {
                currChild = FindWindowEx(hParent, prevChild, null, null);
                if (currChild == IntPtr.Zero) break;
                result.Add(currChild);
                prevChild = currChild;
                ++ct;
            }
            return result;
        }

        public static void ActivateApp(string processName)
        {
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName(processName);

            // Activate the first application we find with this name
            if (p.Length > 0)
                SetForegroundWindow(p[0].MainWindowHandle);
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
