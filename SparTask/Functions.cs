/* DeeShell - A shell replacement for Windows
 * Pravin Paratey (February 19, 2007)
 * 
 * Article: http://www.dustyant.com/articles/deeshell
 * 
 * Released under Creative Commons Attribution 2.5 Licence
 * http://creativecommons.org/licenses/by/2.5/
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

		/// <summary>
		/// Resizes the Desktop area to our shells' requirements
		/// </summary>
		public static void MakeNewDesktopArea()
		{
			// Save current Working Area size
			m_rcOldDesktopRect.left = SystemInformation.WorkingArea.Left;
			m_rcOldDesktopRect.top = SystemInformation.WorkingArea.Top;
			m_rcOldDesktopRect.right = SystemInformation.WorkingArea.Right;
			m_rcOldDesktopRect.bottom = SystemInformation.WorkingArea.Bottom;

			// Make a new Workspace
			WinAPI.RECT rc;
			rc.left = SystemInformation.VirtualScreen.Left;
			rc.top = SystemInformation.VirtualScreen.Top; // We reserve the 24 pixels on top for our taskbar
			rc.right = SystemInformation.VirtualScreen.Right;
			rc.bottom = SystemInformation.VirtualScreen.Bottom - 40;
			WinAPI.SystemParametersInfo((int)WinAPI.SPI.SPI_SETWORKAREA, 0, ref rc, 0);
		}

		/// <summary>
		/// Restores the Desktop area
		/// </summary>
		public static void RestoreDesktopArea()
		{
			WinAPI.SystemParametersInfo((int)WinAPI.SPI.SPI_SETWORKAREA, 0, ref m_rcOldDesktopRect, 0);
		}

		/// <summary>
		/// Hides the Windows Taskbar
		/// </summary>
		public static void HideTaskBar()
		{
			// Get the Handle to the Windows Taskbar
			m_hTaskBar = WinAPI.FindWindow("Shell_TrayWnd", null);
			// Hide the Taskbar
			if (m_hTaskBar != IntPtr.Zero)
			{
				WinAPI.ShowWindow(m_hTaskBar, (int)WinAPI.WindowShowStyle.Hide);
			}
		}

		/// <summary>
		/// Show the Windows Taskbar
		/// </summary>
		public static void ShowTaskBar()
		{
			if (m_hTaskBar != IntPtr.Zero)
			{
				WinAPI.ShowWindow(m_hTaskBar, (int)WinAPI.WindowShowStyle.Show);
			}
		}

        /// <summary>
        /// Gets a list of Active Tasks
        /// </summary>
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
                    //ITEM.Add(Convert.ToString(ExStyle, 8));
                    ITEM.Add(strTitle);
                    ITEM.Add(hWnd);
                    uint pid = 0;
                    WinAPI.GetWindowThreadProcessId(hWnd, out pid);
                    System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById((int)pid);
                    ITEM.Add(proc);
                    SparTaskButtohProc.Add(proc.ProcessName);
                    SparTaskButtonInfoTbl.Add(ITEM);
                //Console.WriteLine(ExStyle & 0x00040000);

                }
                return true;
            };

            returned.Add(SparTaskButtonInfoTbl);
            returned.Add(SparTaskButtohProc);

            if (WinAPI.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
            {
               foreach (var item in SparTaskAppsNames)
                {
                    //Console.WriteLine(item);
                }
            }
            return returned;

        }
	}
}
