using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Instrumentation;
using System.Management;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace wrMainAntiRansomeware
{
    public partial class bkupSetting : Form
    {
        public static Form AForm;

        public static bool AllowStartVSS = false;
        public static object lk = new object();
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public class SERVICE_NOTIFY
        {
            public uint dwVersion;
            public IntPtr pfnNotifyCallback;
            public IntPtr pContext;
            public uint dwNotificationStatus;
            public SERVICE_STATUS_PROCESS ServiceStatus;
            public uint dwNotificationTriggered;
            public IntPtr pszServiceNames;
        };

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct SERVICE_STATUS_PROCESS
        {
            public uint dwServiceType;
            public uint dwCurrentState;
            public uint dwControlsAccepted;
            public uint dwWin32ExitCode;
            public uint dwServiceSpecificExitCode;
            public uint dwCheckPoint;
            public uint dwWaitHint;
            public uint dwProcessId;
            public uint dwServiceFlags;
        };

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenSCManager(string machineName, string databaseName, uint dwAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern uint NotifyServiceStatusChange(IntPtr hService, uint dwNotifyMask, IntPtr pNotifyBuffer);

        [DllImportAttribute("kernel32.dll", EntryPoint = "SleepEx")]
        public static extern uint SleepEx(uint dwMilliseconds, [MarshalAsAttribute(UnmanagedType.Bool)] bool bAlertable);

        public static SERVICE_NOTIFY notify;
        public static GCHandle notifyHandle;
        public static IntPtr unmanagedNotifyStructure;
        static volatile StatusChanged changeDelegate = ReceivedStatusChangedEvent;
        static bool renew = false;
        public static bool renew2 = false;
        public static void ProtectSrv()
        {
            try
            {
                IntPtr hSCM = OpenSCManager(null, null, (uint)0xF003F);
                if (hSCM != IntPtr.Zero)
                {
                    IntPtr hService = OpenService(hSCM, "VSS", (uint)0xF003F);
                    if (hService != IntPtr.Zero)
                    {



                        notify = new SERVICE_NOTIFY();
                        notify.dwVersion = 2;
                        notify.pfnNotifyCallback = Marshal.GetFunctionPointerForDelegate(changeDelegate);
                        notify.pContext = IntPtr.Zero;
                        notify.dwNotificationStatus = 0;
                        SERVICE_STATUS_PROCESS process;
                        process.dwServiceType = 0;
                        process.dwCurrentState = 0;
                        process.dwControlsAccepted = 0;
                        process.dwWin32ExitCode = 0;
                        process.dwServiceSpecificExitCode = 0;
                        process.dwCheckPoint = 0;
                        process.dwWaitHint = 0;
                        process.dwProcessId = 0;
                        process.dwServiceFlags = 0;
                        notify.ServiceStatus = process;
                        notify.dwNotificationTriggered = 0;
                        notify.pszServiceNames = Marshal.StringToHGlobalUni("VSS");
                        notifyHandle = GCHandle.Alloc(notify, GCHandleType.Pinned);
                        unmanagedNotifyStructure = notifyHandle.AddrOfPinnedObject();


                        NotifyServiceStatusChange(hService, (uint)0x00000002, unmanagedNotifyStructure);
                        GC.KeepAlive(changeDelegate);
                        lock (Form1.lk)
                        {
                            renew2 = true;
                        }
                        //  Console.ReadLine();
                        while (true)
                        {
                            lock (lk)
                            {
                                if (renew)
                                {
                                    NotifyServiceStatusChange(hService, (uint)0x00000002, unmanagedNotifyStructure);




                                    renew = false;
                                }
                            }
                            SleepEx(100, true);
                        }

                        //  Console.WriteLine("Waiting for the service to stop. Press enter to exit.");

                        notifyHandle.Free();
                    }
                }
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
           
        }
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void StatusChanged(IntPtr parameter);
        
        public static void ReceivedStatusChangedEvent(IntPtr parameter)
        {
            try
            {
                lock (lk)
                {
                    if (!AllowStartVSS)
                    {
                        string cmd1 = "net stop VSS";
                        //string cmd2 = "sc.exe config VSS start= disabled";
                        var powershell = PowerShell.Create();
                        //powershell.Commands.AddScript(command1);
                        //powershell.Commands.AddScript(command2);
                        powershell.Commands.AddScript(cmd1);
                        //powershell.Commands.AddScript(cmd2);
                       // powershell.Invoke();

                    }

                    renew = true;
                    GC.KeepAlive(changeDelegate);
                }
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
          
// ProtectSrv();
            //MessageBox.Show("Service started.");
        }

    

    public bkupSetting()
        {
            InitializeComponent(); AForm = this;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(Environment.CurrentDirectory + "\\tmsetting", numericUpDown1.Value.ToString(), new UTF8Encoding(false));
                File.WriteAllText(Environment.CurrentDirectory + "\\bcsetting", numericUpDown2.Value.ToString(), new UTF8Encoding(false));
                /*var runSpace = RunspaceFactory.CreateRunspace();
                runSpace.Open();

                runSpace.SessionStateProxy.SetVariable("item", "FooBar");
                var a = runSpace.SessionStateProxy.PSVariable.GetValue("item");*/
                button1.Enabled = false;
                Form1.timer1.Interval = 60 * 1000  * 60 * Int32.Parse(numericUpDown1.Value.ToString());
                Form1.MaxbkupCount = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && d.TotalFreeSpace > ((long)freespaceMin) && d.IsReady == true).Count()*Int32.Parse(numericUpDown2.Value.ToString());
                Form1.timer1.Stop();
                Form1.timer1.Start();
                MessageBox.Show("Success!");
                button1.Enabled = true;
                this.Close();
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
          
            //Assert.IsTrue(a.ToString() == "FooBar");
        }
        public static ulong freespaceMin = (long)1024 * 1024 * 1024 * 2;
        private void bkupSetting_Load(object sender, EventArgs e)
        {
            try
            {
                AForm = this;
                if (File.Exists(Environment.CurrentDirectory + "\\tmsetting"))
                {
                    numericUpDown1.Value = Decimal.Parse(File.ReadAllText(Environment.CurrentDirectory + "\\tmsetting"));
                    Form1.timer1.Interval = 60 * 1000  * 60 * Int32.Parse(numericUpDown1.Value.ToString());
                }
                if (File.Exists(Environment.CurrentDirectory + "\\bcsetting"))
                {
                    numericUpDown2.Value = Decimal.Parse(File.ReadAllText(Environment.CurrentDirectory + "\\bcsetting"));
                    Form1.MaxbkupCount = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && d.TotalFreeSpace > ((long)freespaceMin) && d.IsReady == true).Count()*Int32.Parse(numericUpDown2.Value.ToString());
                }
                if (File.Exists(Environment.CurrentDirectory + "\\frssetting"))
                {
                    textBox1.Text = File.ReadAllText(Environment.CurrentDirectory + "\\frssetting");
                    bkupSetting.freespaceMin = ulong.Parse(textBox1.Text);
                }
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
         
        }

        private void bkupSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void button1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
