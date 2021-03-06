﻿/* ORIGINAL CREDITS
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
using System.Windows.Forms;
using System.IO;

namespace SparTask
{
    partial class Taskbar
    {
        private System.ComponentModel.IContainer components = null;

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SparTaskBar = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonAddIcon = new System.Windows.Forms.Button();
            this.ButtonExit = new System.Windows.Forms.Button();
            this.SparTaskBar.SuspendLayout();
            this.SuspendLayout();
            this.SparTaskBar.ColumnCount = 100;
            this.SparTaskBar.Controls.Add(this.ButtonAddIcon, 1, 0);
            this.SparTaskBar.Controls.Add(this.ButtonExit, 0, 0);
            this.SparTaskBar.Location = new System.Drawing.Point(0, 0);
            this.SparTaskBar.Margin = new System.Windows.Forms.Padding(0);
            this.SparTaskBar.Name = "SparTaskBar";
            this.SparTaskBar.RowCount = 1;
            this.SparTaskBar.AllowDrop = true;
            this.SparTaskBar.Size = new System.Drawing.Size(1000, 40);
            this.SparTaskBar.TabIndex = 0;
            this.SparTaskBar.DragOver += SparTaskBar_DragOver;
            this.SparTaskBar.DragDrop += SparTaskBar_DragDrop;

            void SparTaskBar_DragOver(object sender, DragEventArgs e)
            {
                e.Effect = DragDropEffects.Move;
            }

            void SparTaskBar_DragDrop(object sender, DragEventArgs e)
            {
                System.Drawing.Point point = this.SparTaskBar.PointToClient(new System.Drawing.Point(e.X, e.Y));
                Control ctrl = this.SparTaskBar.GetChildAtPoint(point);
                int index = this.SparTaskBar.Controls.Count;
                if (ctrl != null) index = this.SparTaskBar.GetPositionFromControl(ctrl).Column;

                if (index < 2) index = 2;
                Control data = this.LastControl;
                this.SparTaskBar.Controls.SetChildIndex(data, index);
                this.LastControl = null;
            }

            var fm = new System.Drawing.FontFamily("HelveticaNeueCyr");
            this.ButtonAddIcon.Location = new System.Drawing.Point(0, 0);
            this.ButtonAddIcon.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonAddIcon.Name = "SparTask.AddButton";
            this.ButtonAddIcon.Size = new System.Drawing.Size(20, 40);
            this.ButtonAddIcon.TabIndex = 0;
            this.ButtonAddIcon.Text = "+";
            this.ButtonAddIcon.Font = new System.Drawing.Font(fm, 12, System.Drawing.FontStyle.Regular);
            this.ButtonAddIcon.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.ButtonAddIcon.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.ButtonAddIcon.FlatStyle = FlatStyle.Flat;
            this.ButtonAddIcon.FlatAppearance.BorderSize = 0;
            this.ButtonAddIcon.MouseUp += new MouseEventHandler(this.SparTaskAddProgram);

            this.ButtonExit.Location = new System.Drawing.Point(0, 0);
            this.ButtonExit.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonExit.Name = "SparTask.ExitButton";
            this.ButtonExit.Size = new System.Drawing.Size(20, 40);
            this.ButtonExit.TabIndex = 0;
            this.ButtonExit.Text = "x";
            this.ButtonExit.Font = new System.Drawing.Font(fm, 12, System.Drawing.FontStyle.Regular);
            this.ButtonExit.BackColor = System.Drawing.Color.FromArgb(0, 0, 0);
            this.ButtonExit.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.ButtonExit.FlatStyle = FlatStyle.Flat;
            this.ButtonExit.FlatAppearance.BorderSize = 0;
            this.ButtonExit.MouseClick += this.ButtonExit_MouseClick;

            this.ButtonHideAll = new System.Windows.Forms.Button();
            this.ButtonHideAll.Location = new System.Drawing.Point(SystemInformation.VirtualScreen.Width - 15, 0);
            this.ButtonHideAll.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonHideAll.Name = "SparTask.ExitButton";
            this.ButtonHideAll.Size = new System.Drawing.Size(15, 40);
            this.ButtonHideAll.TabIndex = 0;
            this.ButtonHideAll.Text = "⇂";
            this.ButtonHideAll.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ButtonHideAll.AutoSize = false;
            this.ButtonHideAll.Font = new System.Drawing.Font(fm, 25, System.Drawing.FontStyle.Bold);
            this.ButtonHideAll.BackColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.ButtonHideAll.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.ButtonHideAll.FlatStyle = FlatStyle.Flat;
            this.ButtonHideAll.FlatAppearance.BorderSize = 0;
            this.ButtonHideAll.Click += SparTaskHideAll;




            var BG1 = new Panel();

            BG1.Location = new System.Drawing.Point(SystemInformation.VirtualScreen.Width - 15 - 40 - 100, 0);
            BG1.BackColor = System.Drawing.Color.FromArgb(80, 80, 80);
            BG1.Size = new System.Drawing.Size(100, 40);

            var BG2 = new Panel();

            BG2.Location = new System.Drawing.Point(SystemInformation.VirtualScreen.Width - 15 - 40, 0);
            BG2.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            BG2.Size = new System.Drawing.Size(40, 40);


            this.LabelTime = new System.Windows.Forms.Label();
            this.LabelTime.Text = DateTime.Now.ToString("H:mm:ss\n") + DateTime.Now.Day + " " + DateTime.Now.DayOfWeek.ToString().Remove(3, DateTime.Now.DayOfWeek.ToString().Length - 3);
            this.LabelTime.Height = 40;
            this.LabelTime.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.LabelTime.Font = new System.Drawing.Font(fm, 12, System.Drawing.FontStyle.Bold);
            this.LabelTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelTime.AutoSize = false;
            this.LabelTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.LabelLang = new System.Windows.Forms.Label();
            this.LabelLang.Text = InputLanguage.CurrentInputLanguage.Culture.ThreeLetterWindowsLanguageName.ToUpper().Remove(2, 1);
            this.LabelLang.Height = 40;
            this.LabelLang.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            this.LabelLang.Font = new System.Drawing.Font(fm, 12, System.Drawing.FontStyle.Bold);
            this.LabelLang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelLang.AutoSize = false;
            this.LabelLang.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    this.ThinkHook();
                    System.Threading.Thread.Sleep(100);
                }
            });

            // 
            // Taskbar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.ClientSize = new System.Drawing.Size(1000, 40);
            this.ControlBox = false;
            this.Controls.Add(this.SparTaskBar);

            this.Controls.Add(BG1);
            this.Controls.Add(BG2);
            this.Controls.Add(ButtonHideAll);
            BG1.Controls.Add(this.LabelTime);
            BG2.Controls.Add(this.LabelLang);
            this.Name = "SparTask.Panel";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.SparTaskBar.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public void SparTaskBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || !System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift)) return;
            this.LastControl = (Control)sender;
            this.SparTaskBar.DoDragDrop(sender, DragDropEffects.Move);
        }

        public void SparTaskHideAll(object sender, EventArgs e)
        {
            IntPtr lHwnd = WinAPI.FindWindow("Shell_TrayWnd", null);
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift)) WinAPI.SendMessageTimeout(lHwnd, WinAPI.WM_COMMAND, (IntPtr)WinAPI.MIN_ALL_UNDO, IntPtr.Zero);
            else WinAPI.SendMessageTimeout(lHwnd, WinAPI.WM_COMMAND, (IntPtr)WinAPI.MIN_ALL, IntPtr.Zero);
        }

        public void ThinkHook()
        {
            if (this.LabelTime.InvokeRequired)
            {
                this.LabelTime.BeginInvoke(new Action(() => this.LabelTime.Text = DateTime.Now.ToString("H:mm:ss\n") + DateTime.Now.Day + " " + DateTime.Now.DayOfWeek.ToString().Remove(3, DateTime.Now.DayOfWeek.ToString().Length - 3)));
            }
            else
            {
                this.LabelTime.Text = DateTime.Now.ToString("H:mm:ss\n") + DateTime.Now.Day + " " + DateTime.Now.DayOfWeek.ToString().Remove(3, DateTime.Now.DayOfWeek.ToString().Length - 3);
            }
            if (this.LabelLang.InvokeRequired)
            {
                var cult = WinAPI.GetCurrentCulture();
                if (!cult.Equals(IntPtr.Zero)) this.LabelLang.BeginInvoke(new Action(() => this.LabelLang.Text = cult.ThreeLetterWindowsLanguageName.ToUpper().Remove(2, 1)));
            }
            else
            {
                var cult = WinAPI.GetCurrentCulture();
                if (!cult.Equals(IntPtr.Zero)) this.LabelLang.Text = cult.ThreeLetterWindowsLanguageName.ToUpper().Remove(2, 1);
            }

        }

        private TableLayoutPanel SparTaskBar;
        private Button ButtonExit;
        private Button ButtonAddIcon;
        private Button ButtonHideAll;
        private Label LabelTime;
        private Label LabelLang;
        public Control LastControl;
    }
}