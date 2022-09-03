using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class AddWhiteList : Form
    {
        public static Form AForm;

        public AddWhiteList()
        {
            InitializeComponent(); AForm = this;
        }

        private void AddWhiteList_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Form1.ad2w = null;
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
         
        }

        private void AddWhiteList_Load(object sender, EventArgs e)
        {
            try
            {
                AForm = this;
                textBox1.Text = File.ReadAllText("..\\trustedsigns.txt");
                button1.Select();
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
          
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GenerateConsoleCtrlEvent(ConsoleCtrlEvent sigevent, int dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);
        delegate Boolean ConsoleCtrlDelegate(ConsoleCtrlEvent type);
        public enum ConsoleCtrlEvent
        {
            CTRL_C = 0,
            CTRL_BREAK = 1,
            CTRL_CLOSE = 2,
            CTRL_LOGOFF = 5,
            CTRL_SHUTDOWN = 6
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText("..\\trustedsigns.txt", textBox1.Text.Trim('\r', '\n'), new UTF8Encoding(false));
                button1.Enabled = false;
                Task.Factory.StartNew((Action)delegate ()
                {
                   /* try
                    {
                        FreeConsole();
                        Process p = Process.GetProcessesByName("WRAREngine")[0];
                        if(AttachConsole((uint)p.Id)){
                        //uint spid= p.SessionId;

                        // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                        SetConsoleCtrlHandler(null, true);

                        //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                        GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                        //
                        Thread.Sleep(2000);
                        FreeConsole();
                        SetConsoleCtrlHandler(null, false);}
                    }
                    catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
                    */
                }).ContinueWith((a) =>
                {
                    MessageBox.Show("The changes were saved successfully!");
                    this.BeginInvoke((Action)delegate ()
                    {
                        button1.Enabled = true;
                        this.Close();
                    });
                });
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
           
        
        }
    }
}
