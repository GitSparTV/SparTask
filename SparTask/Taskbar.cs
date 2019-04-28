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
using System.Management;
using System.Data;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace SparTask
{
    public partial class Taskbar : Form
    {



        public static ArrayList SparTaskButtonTbl = new ArrayList();
        public static object[] SparTaskButtonInfoTbl;
        public static object[] SparTaskButtohProc;

        public Taskbar()
        {
            InitializeComponent();
            WinAPI.SetWindowPos(this.Handle, IntPtr.Zero, 0, SystemInformation.VirtualScreen.Height - 40, SystemInformation.VirtualScreen.Width, 40, 0x0040);
            SparTaskUpdateAppList();
            SparTaskHookUpdate();
        }

        public static void SparTaskUpdateAppList()
        {
            ArrayList ar = Functions.GetActiveTasks();
            SparTaskButtonInfoTbl = ((ArrayList)ar[0]).ToArray();
            SparTaskButtohProc = ((ArrayList)ar[1]).ToArray();
            Array.Sort(SparTaskButtohProc, SparTaskButtonInfoTbl);
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SparTaskAddProgram(object sender, MouseEventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                var menu = new ContextMenuStrip();
                for (int i = 0; i < SparTaskButtonInfoTbl.Length; i++)
                {
                    bool create = false;
                    foreach (Button b in SparTaskButtonTbl) { if (((ArrayList)b.Tag)[1].Equals(((ArrayList)SparTaskButtonInfoTbl[i])[1])) create = true; }
                    if (create) continue;
                    var item = new ToolStripMenuItem((string)(((ArrayList)SparTaskButtonInfoTbl[i])[0]));
                    Icon IEIcon = System.Drawing.Icon.ExtractAssociatedIcon(WinAPI.GetProcessPath((IntPtr)(((ArrayList)SparTaskButtonInfoTbl[i])[1])));
                    Image im = IEIcon.ToBitmap();
                    item.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
                    item.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
                    item.Image = im;
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() => menu.Items.Add(item)));
                        this.BeginInvoke(new Action(() => menu.Show(this, new Point(e.X, e.Y))));
                    }
                    else
                    {
                        menu.Items.Add(item);
                        menu.Show(this, new Point(e.X, e.Y));
                    }
                    item.Tag = ((ArrayList)SparTaskButtonInfoTbl[i]);
                    item.Click += new EventHandler(this.SparTaskAddProgram_);
                }
                return;
            });
        }

        private void SparTaskAddProgram_(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            ArrayList Item = (ArrayList)item.Tag;
            try
            {
                Icon IEIcon = System.Drawing.Icon.ExtractAssociatedIcon(WinAPI.GetProcessPath((IntPtr)Item[1]));
                System.Windows.Forms.Button SparTaskButton = new System.Windows.Forms.Button();
                SparTaskButtonTbl.Add(SparTaskButton);
                Image im = IEIcon.ToBitmap();
                SparTaskButton.Name = "TaskBarButton." + (string)Item[0];
                SparTaskButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
                SparTaskButton.Image = im;
                SparTaskButton.Tag = Item;
                uint pid = 0;
                WinAPI.GetWindowThreadProcessId((IntPtr)Item[1], out pid);
                System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById((int)pid);
                proc.EnableRaisingEvents = true;
                proc.Exited += (sender2, event2) => this.SparTaskHookRemoveButton(sender2, event2, SparTaskButton);
                SparTaskButton.Location = new System.Drawing.Point(0, 0);
                SparTaskButton.Margin = new System.Windows.Forms.Padding(0);
                SparTaskButton.Size = new System.Drawing.Size(40, 40);
                SparTaskButton.TabIndex = 0;
                SparTaskButton.FlatStyle = FlatStyle.Flat;
                SparTaskButton.FlatAppearance.BorderSize = 0;
                System.Windows.Forms.ToolTip tt = new System.Windows.Forms.ToolTip();
                tt.SetToolTip(SparTaskButton, (string)Item[0]);
                SparTaskButton.MouseUp += this.SparTaskSwitchTo;
                SparTaskButton.MouseDown += SparTaskBar_MouseDown;
                SparTaskBar.Controls.Add(SparTaskButton);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                Console.WriteLine("Error in: " + ((string)Item[0]));
            }
        }

        private void SparTaskSwitchTo(object sender, MouseEventArgs e)
        {

            System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;
            ArrayList Item = (ArrayList)b.Tag;
            if (e.Button == MouseButtons.Left)
            {
                WinAPI.SwitchToThisWindow((IntPtr)Item[1], true);
            }
            else if (e.Button == MouseButtons.Right)
            {
                var menu = new ContextMenu();
                var item2 = new MenuItem("Kill");
                menu.MenuItems.Add(item2);
                item2.Tag = b;
                item2.Click += new EventHandler(this.SparTaskMenuKill);

                var item1 = new MenuItem("Remove");
                menu.MenuItems.Add(item1);
                item1.Tag = b;
                item1.Click += new EventHandler(this.SparTaskMenuRemove);

                menu.Show(b, new Point(e.X, e.Y));
            }
        }

        private void SparTaskMenuRemove(object sender, EventArgs e)
        {
            System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem)sender;
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)item.Tag;
            SparTaskBar.Controls.Remove(b);
        }
        private void SparTaskMenuKill(object sender, EventArgs e)
        {
            System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem)sender;
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)item.Tag;
            ArrayList Item = (ArrayList)b.Tag;
            try
            {
                uint pid = 0;
                WinAPI.GetWindowThreadProcessId((IntPtr)Item[1], out pid);
                System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById((int)pid); //Gets the process by ID. 
                proc.Kill();
                SparTaskBar.Controls.Remove(b);
            }
            catch
            {
                Console.Beep();
            }

        }

        public static ManagementEventWatcher startWatch;
        public static ManagementEventWatcher stopWatch;

        public void SparTaskHookUpdate()
        {
            ManagementEventWatcher startWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(SparTaskHookProcessUpdate);
            startWatch.Start();

            ManagementEventWatcher stopWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(SparTaskHookProcessUpdate);
            stopWatch.Start();
        }

        public void SparTaskHookRemoveButton(object sender, EventArgs e, System.Windows.Forms.Button button)
        {
            if (button.InvokeRequired)
            {
                button.Invoke(new Action(() => this.SparTaskBar.Controls.Remove(button)));
            }
            else
            {
                this.SparTaskBar.Controls.Remove(button);
            }
        }

        public static void SparTaskHookProcessUpdate(object sender, EventArrivedEventArgs e)
        {
            SparTaskUpdateAppList();
        }
    }
}