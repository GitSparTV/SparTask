/* ORIGINAL CREDITS
 * 
 * DeeShell - A shell replacement for Windows
 * Pravin Paratey (February 19, 2007)
 * 
 * https://github.com/pravin/deeshell
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
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

namespace SparTask
{
    class Functions
    {
        #region Private variables
        private static WinAPI.RECT m_rcOldDesktopRect;
        private static IntPtr m_hTaskBar;
        #endregion

        public static void MakeNewDesktopArea()
        {
            m_rcOldDesktopRect.left = SystemInformation.WorkingArea.Left;
            m_rcOldDesktopRect.top = SystemInformation.WorkingArea.Top;
            m_rcOldDesktopRect.right = SystemInformation.WorkingArea.Right;
            m_rcOldDesktopRect.bottom = SystemInformation.WorkingArea.Bottom;

            WinAPI.RECT rc;
            rc.left = SystemInformation.VirtualScreen.Left;
            rc.top = SystemInformation.VirtualScreen.Top;
            rc.right = SystemInformation.VirtualScreen.Right;
            rc.bottom = SystemInformation.VirtualScreen.Bottom - 40;
            WinAPI.SystemParametersInfo((int)WinAPI.SPI.SPI_SETWORKAREA, 0, ref rc, 0);
        }

        public static void RestoreDesktopArea()
        {
            WinAPI.SystemParametersInfo((int)WinAPI.SPI.SPI_SETWORKAREA, 0, ref m_rcOldDesktopRect, 0);
        }

        public static void HideTaskBar()
        {
            m_hTaskBar = WinAPI.FindWindow("Shell_TrayWnd", null);
            if (m_hTaskBar != IntPtr.Zero)
            {
                WinAPI.ShowWindow(m_hTaskBar, (int)WinAPI.WindowShowStyle.Hide);
            }
        }

        public static void ShowTaskBar()
        {
            if (m_hTaskBar != IntPtr.Zero)
            {
                WinAPI.ShowWindow(m_hTaskBar, (int)WinAPI.WindowShowStyle.Show);
            }
        }

        public static ArrayList GetActiveTasks()
        {
            ArrayList returned = new ArrayList();
            ArrayList SparTaskAppsNames = new ArrayList();
            ArrayList SparTaskAppshWnd = new ArrayList();
            ArrayList SparTaskButtohProc = new ArrayList();
            ArrayList SparTaskButtonInfoTbl = new ArrayList();
            WinAPI.EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
            {
                ArrayList ITEM = new ArrayList();
                StringBuilder strbTitle = new StringBuilder(255);
                int nLength = WinAPI.GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                string strTitle = strbTitle.ToString();
                long Style = (long)WinAPI.GetWindowLong(hWnd, (-16));
                long ExStyle = (long)WinAPI.GetWindowLong(hWnd, (-20));

                if (WinAPI.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false && (ExStyle & 0x00000080L) == 0)
                {
                    ITEM.Add(strTitle);
                    ITEM.Add(hWnd);
                    uint pid = 0;
                    WinAPI.GetWindowThreadProcessId(hWnd, out pid);
                    System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById((int)pid);
                    ITEM.Add(proc);
                    SparTaskButtohProc.Add(proc.ProcessName);
                    SparTaskButtonInfoTbl.Add(ITEM);
                }
                return true;
            };

            returned.Add(SparTaskButtonInfoTbl);
            returned.Add(SparTaskButtohProc);

            if (WinAPI.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero)) { }
            return returned;

        }
    }
}
