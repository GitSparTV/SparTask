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
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SparTask
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Functions.HideTaskBar();
            Functions.MakeNewDesktopArea();

            Application.Run(new Taskbar());

            Functions.RestoreDesktopArea();
            Functions.ShowTaskBar();
        }
    }
}