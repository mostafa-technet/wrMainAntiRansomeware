using AutoUpdaterDotNET;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Globalization;
using System.Reflection;

namespace wrMainAntiRansomeware
{
   
    public partial class Form1 : Form
    {
        public static Form AForm = null;


        [DllImport("Kernel32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE state);

        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 2,
            ES_SYSTEM_REQUIRED = 1,
            ES_AWAYMODE_REQUIRED = 0x00000040
        }

        public Form1()
        {
            if (Environment.OSVersion.Version.Major > 5)
            {
                // vista and above: block suspend mode
                SetThreadExecutionState(EXECUTION_STATE.ES_AWAYMODE_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
            }

            AForm = this;
            InitializeComponent(); AForm = this;
            //Program.runPrTask();
            //  Thread tsk = new Thread(new ThreadStart(Program.runPrTask));
            // tsk.Start();
            //MessageBox.Show(string.Format("version: {0}", Environment.OSVersion.Version.Major.ToString() ));

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (Environment.OSVersion.Version.Major > 5)
            {
                // Re-allow suspend mode
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            }
        }


        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            // Power status event triggered
            if (m.Msg == (int)WindowMessage.WM_POWERBROADCAST)
            {
                // Machine is trying to enter suspended state
                if (m.WParam.ToInt32() == (int)WindowMessage.PBT_APMQUERYSUSPEND ||
                        m.WParam.ToInt32() == (int)WindowMessage.PBT_APMQUERYSTANDBY)
                {
                    // Have perms to deny this message?
                    if ((m.LParam.ToInt32() & 0x1) != 0)
                    {
                        // If so, deny broadcast message
                        m.Result = new IntPtr((int)WindowMessage.BROADCAST_QUERY_DENY);
                    }
                    try
                    {

                        Process.GetProcessesByName("WRAREngine").ToList().ForEach((p) => Process.Start($"..\\ProcessCritical\\ProcessCritical64.exe -pid {p.Id} -CriticalFlag 0"));
                        // Process.GetProcessesByName("WRAREngine").ToList().ForEach((p) => p.Kill());

                    }
                    catch { }
                }
                else if (m.WParam.ToInt32() == (int)WindowMessage.PBT_APMRESUMEAUTOMATIC)
                {
                    Process.Start("..\\WRAREngine.exe");
                }
                return;
            }

            base.WndProc(ref m);
        }




        internal enum WindowMessage
        {

            /// <summary>
            /// Notify that machine power state is changing
            /// </summary>
            WM_POWERBROADCAST = 0x218,
            /// <summary>
            /// Message indicating that machine is trying to enter suspended state
            /// </summary>
            PBT_APMQUERYSUSPEND = 0x0,
            PBT_APMQUERYSTANDBY = 0x0001,
            PBT_APMRESUMEAUTOMATIC = 0x12,
            /// <summary>
            /// Message to deny broadcast query
            /// </summary>
            BROADCAST_QUERY_DENY = 0x424D5144


        }
        private static string unpack(string p1, string input)
        {
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                string a = Convert.ToInt32(input[i]).ToString("X");
                output.Append(a);
            }

            return output.ToString();
        }

        public static byte[] SHexToByteArray(String hex)
        {

            hex = hex.Length % 2 != 0 ? "0" + hex : hex;
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public static string StrToHex(string str)
        {
            var st = str.Replace("-", "");
            //mysqli_close();
            // MessageBox.Show(st);
            StringBuilder rstr = new StringBuilder();
            for (int i = 0; i < st.Length; ++i)
            {
                // MessageBox.Show(st[i]+" "+ Int32.Parse(((int)(st[i] - 'A')).ToString()));
                rstr.Append(Convert.ToString(Int32.Parse(((int)(st[i] - 'A')).ToString()), 16));
            }
            //MessageBox.Show(rstr.ToString());
            return rstr.ToString();
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private static void RunAsDesktopUser(string fileName)
        {
            try
            {
                ProcessStartInfo ps1 = new ProcessStartInfo(".\\PsExec64.exe");
                ps1.Arguments = "-l \"" + fileName + "\"";
                ps1.CreateNoWindow = true;
                ps1.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(ps1);
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }
        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (als == null)
                {
                    als = new AlertSettings();
                    als.Show();

                }
                else
                {
                    als.Show();
                    als.BringToFront();
                }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }
        public static bool runonce = false;
        public static bool runonce2 = false;
        public static object lk = new object();
        static bool runonce1 = false;

        static void appShortcutToDesktop(string linkName, string app = null)
        {
            if (app == null)
                app = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //    MessageBox.Show(app);
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
            {

                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + app);
                writer.WriteLine("IconIndex=0");
                string icon = app.Replace('\\', '/');
                writer.WriteLine("IconFile=" + icon);
            }
        }
        static void MakeFolderLink(string link, string target)
        {
            string cmda = $"/c mklink /D \"{link}\" \"{target}\"";
            ProcessStartInfo psi = new ProcessStartInfo($"cmd.exe", cmda);
            psi.Verb = "runas";
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            var p = Process.Start(psi);
            p.WaitForExit();
        }
        List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
        public static bool IsLicenseValid = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            //       MakeFolderLink(@"C:\Users\Mostafa\Desktop\Webroam Protected HomeDir", @"C:\Users\Mostafa\Documents\");
            AForm = this;
            Task.Run(()=>timer1_Tick(null, null));
            //appShortcutToDesktop("Webroam Protected HomeDir", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

            try
            {
                if(File.Exists("license2") && !File.Exists("license"))
                {
                    File.Move("license2", "license");
                }
                    loadMenue();
                notifyIcon1.ContextMenuStrip = contextMenuStrip2;
                notifyIcon1.Visible = true;
                // MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().Location);
                // MessageBox.Show(Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName);
                string fldr0 = Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName;
                //string fldr1 = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                isMyUpPerm = true;
                //  MessageBox.Show(fldr0);        
                AccessControl.SetAccessFileDenyOrAllow(fldr0, true);
                //AccessControl.SetAccessFileDenyOrAllow(fldr1);
                //MessageBox.Show(fldr0);
                /*var watcher = new FileSystemWatcher(Directory.GetParent(fldr0).FullName);
                watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.Security | NotifyFilters.LastWrite;
                watcher.Filter = "*";
                watcher.IncludeSubdirectories = true;
                // MessageBox.Show(Directory.GetParent(v).FullName);
                
                watcher.Changed += delegate (object a, FileSystemEventArgs b)
                {
                   // MessageBox.Show(b.FullPath);
                    if (!b.FullPath.ToLower().StartsWith(fldr0.ToLower()))
                        return;
                    if (!isMyUpPerm)
                    {
                         // MessageBox.Show(b.FullPath);
                        isMyUpPerm = true;
                        var mywatcher = (FileSystemWatcher)a;
                        mywatcher.EnableRaisingEvents = false;
                        AccessControl.SetAccessFileDenyOrAllow(b.FullPath, true);
                        isMyUpPerm = false;
                        mywatcher.EnableRaisingEvents = true;
                    }
                };
                watcher.EnableRaisingEvents = true;
                watchers.Add(watcher);
                */
                var dri = Directory.EnumerateDirectories(fldr0);
                isMyUpPerm = true;
                AccessControl.SetAccessFileDenyOrAllow(fldr0, true);
                if (dri?.Count() > 0)
                    foreach (var v in dri)
                    {
                        AccessControl.SetAccessFileDenyOrAllow(v, true);
                    }
                isMyUpPerm = false;
                bool registered = false;
                List<string> lser = new List<string>();
                string filename = $"{ System.IO.Directory.GetParent(System.IO.Path.GetDirectoryName(Application.ExecutablePath))}\\app.wrdb";
                //MessageBox.Show(filename);                
                var tbl = Csv.DataSetGet(filename, "?", out lser);
                isMyUpPerm = true;
                try
                {
                    if (tbl.Rows?.Count > 0)
                        foreach (DataRow row in tbl.Rows)
                        {
                            if (row[4].ToString() != "*" && row[3].ToString() != "*")
                            {
                                string fldr = row[3].ToString().Trim();
                                /*if (!fldr.Contains("*"))
                                {*/
                                if (!Directory.Exists(fldr))
                                    continue;
                                bool admin = row[5].ToString().ToUpper().Trim() == "TRUE";

                                var subfs = Directory.EnumerateDirectories(fldr);
                                AccessControl.SetAccessFileDenyOrAllow(fldr, row[0].ToString().ToUpper() == "TRUE" ? admin : false);
                                if (subfs?.Count() > 0)
                                    foreach (var df in subfs)
                                    {
                                        AccessControl.SetAccessFileDenyOrAllow(df, row[0].ToString().ToUpper() == "TRUE" ? admin : false);
                                    }

                                var watcher = new FileSystemWatcher(Directory.GetParent(fldr).FullName);
                                watcher.Path = fldr;
                                watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.Security | NotifyFilters.DirectoryName | NotifyFilters.LastAccess;
                                watcher.Filter = "*";
                                watcher.IncludeSubdirectories = true;
                                watcher.Changed += delegate (object a, FileSystemEventArgs b)
                                {
                                    if (!b.FullPath.ToLower().StartsWith(fldr.ToLower()))
                                        return;
                                    //Mess
                                    if (!isMyUpPerm)
                                    {
                                        var mywatcher = (FileSystemWatcher)a;
                                        isMyUpPerm = true;
                                        mywatcher.EnableRaisingEvents = false;
                                        AccessControl.SetAccessFileDenyOrAllow(b.FullPath, row[0].ToString().ToUpper() == "TRUE" ? admin : false);
                                        isMyUpPerm = false;
                                        mywatcher.EnableRaisingEvents = true;
                                    }

                                };
                                //  MessageBox.Show("1");
                                watcher.EnableRaisingEvents = true;
                                watchers.Add(watcher);


                                //  }
                            }
                        }
                }
                catch
                { }
                finally
                {
                    isMyUpPerm = false;
                }

                for (int i = 0; i < watchers?.Count; i++)
                {
                    new Thread(new ThreadStart(() => {
                        while (true)
                            watchers[i].WaitForChanged(WatcherChangeTypes.Changed);
                    }));
                }


                string[] license;
                string lictx = "";
                string[] lfile = null;
                if (File.Exists("license"))
                {
                    lfile = File.ReadAllLines("license");
                    lictx = lfile[0].Trim();
                }
                if (lfile == null || lfile.Count() < 3)
                {
                    lictx = "";
                    MessageBox.Show("Invalid License!");
                    _isExplicitClose = true;
                    this.Close();
                    return;
                }

                string[] licontent = new string[3];
                licontent[0] = lfile.ElementAt(0);
                licontent[2] = lfile.ElementAt(2);
                string macAddr =
        (
            from nic in NetworkInterface.GetAllNetworkInterfaces()
            where nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet  || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
            select nic?.GetPhysicalAddress().ToString()
        )?.FirstOrDefault();
                if (macAddr != null)
                {
                    licontent[1] = CreateMD5(macAddr);
                    File.WriteAllLines("license", licontent);
                    license = Encoding.UTF8.GetString(Aes256Decrypt(SHexToByteArray(StrToHex(lictx)))).Split(' ');
                    //1 product id
                    //4 license version
                    if (!(license?.Length == 6 && license[1] == "1001" && license[4] == "1"))
                    {
                        MessageBox.Show("Invalid License!");
                        _isExplicitClose = true;
                        this.Close();
                    }
                    /*
                    string mc = (from nic2 in NetworkInterface.GetAllNetworkInterfaces()
                                 where nic2.OperationalStatus == OperationalStatus.Up && nic2.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                                 select nic2?.GetPhysicalAddress().ToString()
            )?.FirstOrDefault();*/


                    if (lfile != null && lfile?.Length > 2 && lfile[2] != "")
                    {
                        string regs = lfile[2];

                        string[] regsarr = Encoding.UTF8.GetString(Aes256Decrypt(SHexToByteArray(StrToHex(regs)))).Split(' ');
                        // MessageBox.Show(CreateMD5(regsarr[5]) +Environment.NewLine+ licontent[1]);
                        //MessageBox.Show(String.Join(" ", regsarr));
                        if (regsarr?.Length == 6 && regsarr[1] == "1001" && regsarr[4] == "1" && CreateMD5(regsarr[5]) == licontent[1])
                        {
                            string year = "20" + regsarr[2].Substring(0, 2);
                            string month = regsarr[2].Substring(2, 2);
                            string period = regsarr[2].Substring(4, 2);
                            DateTime dt = new DateTime(Int32.Parse(year), Int32.Parse(month), DateTime.Now.Day);
                            dt = dt.AddMonths(Int32.Parse(period));
                            //MessageBox.Show(DateTime.Now.Subtract(dt).Days.ToString());
                            if (DateTime.Now.Subtract(dt).Days <= 0)
                                registered = true;
                        }

                    }
                }
                IsLicenseValid = registered;
                if (!registered)
                {


                    if ((DateTime.Now.Month > 11 && DateTime.Now.Year > 2022))
                    {
                        notifyIcon1.ContextMenuStrip = null;
                        notifyIcon1.Visible = false;
                        string[] flines = File.ReadAllLines("..\\app_config.ini");

                        int slevel = int.Parse(flines?.FirstOrDefault((a) => a.Contains("SecurityLevel")).Replace("SecurityLevel", "").Replace("=", "").Trim());
                        var s = (from content in flines where !content.Contains("SecurityLevel") select content)?.FirstOrDefault();
                        string filecntnt = string.IsNullOrEmpty(s) ? "" : String.Join(Environment.NewLine, s);
                        filecntnt += "\nSecurityLevel=3";
                        File.WriteAllText("..\\app_config.ini", filecntnt, new UTF8Encoding(false));
                        /*try
                        {
                                Process p = Process.GetProcessesByName("WrArServ")[0];
                                FreeConsole();
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
                        catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
                        */
                        new ProductKeyForm().Show();
                        return;

                    }
                    else
                    {
                        IsLicenseValid = true;
                    }
                    new ProductKeyForm().Show();
                }

                if (!disableToolStripMenuItem.Checked)
                {
                    timer3.Enabled = true;
                    timer3.Start();
                }
                else
                {
                    disableToolStripMenuItem_Click(null, null);
                }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

            try
            {
                if (runonce1)
                    return;
                runonce1 = true;
                notifyIcon1.Visible = false;
                if (File.Exists(Environment.CurrentDirectory + "\\tmsetting"))
                {
                    Form1.timer1.Interval = 60 * 1000 * 60 * int.Parse(File.ReadAllText(Environment.CurrentDirectory + "\\tmsetting"));
                }
                if (File.Exists(Environment.CurrentDirectory + "\\bcsetting"))
                {
                    Form1.MaxbkupCount = int.Parse(File.ReadAllText(Environment.CurrentDirectory + "\\bcsetting"));// * DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && d.TotalFreeSpace > ((long)freespaceMin) && d.IsReady == true).Count();
                }
                if (File.Exists(Environment.CurrentDirectory + "\\frssetting"))
                {
                    bkupSetting.freespaceMin = ulong.Parse(File.ReadAllText(Environment.CurrentDirectory + "\\frssetting"));// * DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && d.TotalFreeSpace > ((long)freespaceMin) && d.IsReady == true).Count();
                }
                Task.Run(() =>
                {
                    string cmd1 = "net stop VSS";
                    string cmd2 = "sc.exe config VSS start= disabled";
                    var powershell = PowerShell.Create();
                    //powershell.Commands.AddScript(command1);
                    //powershell.Commands.AddScript(command2);
                 //   powershell.Commands.AddScript(cmd1);
                   // powershell.Commands.AddScript(cmd2);
                    //powershell.Invoke();
                }).ContinueWith((t) =>
                {
                    Task.Run(() =>
                    {
                        Task.Run(() => bkupSetting.ProtectSrv());
                        while (true)
                        {
                            lock (Form1.lk)
                            {


                                if (bkupSetting.renew2)
                                    break;
                            }
                            Thread.Sleep(260);
                        }
                    }).ContinueWith((t4) =>
                    {
                        Task.Run(() =>
                        {
                            lock (lk)
                            {
                                if (!runonce)
                                {
                                    runonce = true;

                                }
                            }

                            while (true)
                            {
                                lock (lk)
                                {
                                    if (runonce2)
                                        break;
                                }

                                Thread.Sleep(250);
                            }
                        }).ContinueWith((t2) =>
                        {
                            timer1.Start();
                            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
                            notifyIcon1.Visible = true;
                        });
                    });
                });


                if (!File.Exists("..\\app_config.ini"))
                    return;
               // loadMenue();
                button1.Select();
                Task.Run(()=>{
                    ProcessStartInfo ps1 = new ProcessStartInfo("sc.exe");
                    ps1.Arguments = "config VSS start=demand";
                    ps1.CreateNoWindow = true;
                    ps1.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(ps1).WaitForExit();

                    ProcessStartInfo ps2 = new ProcessStartInfo("net");
                    ps2.Arguments = "start VSS";
                    ps2.CreateNoWindow = true;
                    ps2.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(ps2).WaitForExit();
                });
                //ProcessProtector p = new ProcessProtector();
                //p.WatchForProcessEnd("WRAREngine");
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void loadMenue()
        {
            string[] flines = File.ReadAllLines("..\\app_config.ini");

            int slevel = int.Parse(flines?.FirstOrDefault((a) => a.Contains("SecurityLevel")).Replace("SecurityLevel", "").Replace("=", "").Trim());
            switch (slevel)
            {
                case 2:
                    if (!mediumToolStripMenuItem.Checked)
                    {
                        mediumToolStripMenuItem.Checked = true;
                        mediumToolStripMenuItem.Font = new Font(mediumToolStripMenuItem.Font, FontStyle.Bold);
                        highToolStripMenuItem.Font = new Font(highToolStripMenuItem.Font, FontStyle.Regular);
                        disableToolStripMenuItem.Font = new Font(disableToolStripMenuItem.Font, FontStyle.Regular);
                        highToolStripMenuItem.Checked = disableToolStripMenuItem.Checked = false;
                        timer3.Enabled = true;
                        timer3.Start();
                    }
                    break;
                case 3:
                    if (!disableToolStripMenuItem.Checked)
                    {
                        disableToolStripMenuItem.Checked = true;
                        disableToolStripMenuItem.Font = new Font(disableToolStripMenuItem.Font, FontStyle.Bold);
                        mediumToolStripMenuItem.Font = new Font(mediumToolStripMenuItem.Font, FontStyle.Regular);
                        highToolStripMenuItem.Font = new Font(highToolStripMenuItem.Font, FontStyle.Regular);
                        mediumToolStripMenuItem.Checked = highToolStripMenuItem.Checked = false;
                    }
                    break;
                case 1:
                    if (!highToolStripMenuItem.Checked)
                    {
                        highToolStripMenuItem.Checked = true;
                        highToolStripMenuItem.Font = new Font(highToolStripMenuItem.Font, FontStyle.Bold);
                        mediumToolStripMenuItem.Font = new Font(mediumToolStripMenuItem.Font, FontStyle.Regular);
                        disableToolStripMenuItem.Font = new Font(disableToolStripMenuItem.Font, FontStyle.Regular);
                        mediumToolStripMenuItem.Checked = disableToolStripMenuItem.Checked = false;
                        timer3.Enabled = true;
                        timer3.Start();
                    }
                    break;
            }

            int balert = int.Parse(flines?.FirstOrDefault((a) => a.Contains("AlertBlock")).Replace("AlertBlock", "").Replace("=", "").Trim());
            if (balert == 1)
                disableAlertsToolStripMenuItem.Checked = false;
            else
                disableAlertsToolStripMenuItem.Checked = true;

            bool selfprotected = int.Parse(flines?.FirstOrDefault((a) => a.Contains("SelfProtection")).Replace("SelfProtection", "").Replace("=", "").Trim()) == 1;
            toolStripMenuItem1.Checked = selfprotected;

            bool protectfromconsole = int.Parse(flines?.FirstOrDefault((a) => a.Contains("DisableConsoleInFormats")).Replace("DisableConsoleInFormats", "").Replace("=", "").Trim()) == 1;
            toolStripMenuItem2.Checked = protectfromconsole;
            string[] slines = flines?.FirstOrDefault((a) => a.Contains("ProtectedExtensions")).Replace("ProtectedExtensions", "").Replace("=", "").Replace(",", Environment.NewLine).Split('\r', '\n');
            if (slines?.Length > 0)
                slines = slines.Select(a => a = a.Trim())?.ToArray();
            if (slines?.Length > 0)
                slines = (from s in slines where s != "" select s)?.ToArray();
            //MessageBox.Show(slines[0]);
            if (slines?.Length > 0)
                this.textBox1.Text = string.Join(Environment.NewLine, slines);
        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        private static string hex2bin(string hexdata)
        {
            if (hexdata == null)
                throw new ArgumentNullException("hexdata");
            if (hexdata.Length % 2 != 0)
                throw new ArgumentException("hexdata should have even length");

            byte[] bytes = new byte[hexdata.Length / 2];
            for (int i = 0; i < hexdata.Length; i += 2)
                bytes[i / 2] = (byte)(HexValue(hexdata[i]) * 0x10
                + HexValue(hexdata[i + 1]));
            return Encoding.GetEncoding(1252).GetString(bytes);
        }

        private static int HexValue(char c)
        {
            int ch = (int)c;
            if (ch >= (int)'0' && ch <= (int)'9')
                return ch - (int)'0';
            if (ch >= (int)'a' && ch <= (int)'f')
                return ch - (int)'a' + 10;
            if (ch >= (int)'A' && ch <= (int)'F')
                return ch - (int)'A' + 10;
            throw new ArgumentException("Not a hexadecimal digit.");
        }


        public static byte[] Aes256Decrypt(byte[] v)
        {
            var cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            var key = ASCIIEncoding.ASCII.GetBytes(("Th@Wbroam$K12317" + "1001" + "    " + "12345678"));

            /* //pad key out to 32 bytes (256bits) if its too short
             if (key.Length < 32)
             {
                 var paddedkey = new byte[32];
                 Buffer.BlockCopy(key, 0, paddedkey, 0, key.Length);
                 key = paddedkey;
             }*/

            //setup an empty iv
            // var iv = new byte[16];

            //get the encrypted data and decrypt
            //byte[] encryptedBytes = SHexToByteArray(StrToHex("VDFUUR-JJEURA-IGFRAR-TTGJTJ-HBSFVB"));
            var crte = new KCtrBlockCipher(new AesEngine());
            var encryptKeyParameter = new KeyParameter(key);
            cipher.Init(false, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", key), ASCIIEncoding.ASCII.GetBytes("1234567812345678")));
            byte[] decr = new byte[1024];
            int len = cipher.ProcessBytes(v, decr, 0);
            len += cipher.DoFinal(decr, len);
            Array.Resize(ref decr, len);
            return decr;
        }

        bool _isExplicitClose = false;
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _isExplicitClose = true;
                notifyIcon1.Visible = false;
                foreach (var s in ProductKeyForm.sw.Values)
                    s.Close();
                Environment.Exit(1);
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!_isExplicitClose)
                {
                    e.Cancel = true;
                    this.Visible = false;
                    string[] flines = File.ReadAllLines("..\\app_config.ini");
                    //MessageBox.Show(String.Join(Environment.NewLine, lines));
                    string[] slines = flines?.FirstOrDefault((a) => a.Contains("ProtectedExtensions")).Replace("ProtectedExtensions", "").Replace("=", "").Replace(",", Environment.NewLine).Split('\r', '\n');
                    slines = slines.Select(a => a = a.Trim())?.ToArray();
                    slines = (from s in slines where s != "" select s)?.ToArray();
                    //MessageBox.Show(slines[0]);
                    if (slines?.Length > 0)
                        this.textBox1.Text = string.Join(Environment.NewLine, slines);
                }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;


            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Opacity = 100;
                this.Show();
                this.TopMost = true;
                this.BringToFront();
                this.TopMost = false;
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void fileFormatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Opacity = 100;
                this.Show();
                this.TopMost = true;
                this.BringToFront();
                this.TopMost = false;
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }


        public static void SetAccessRights(string file)
        {
            FileSecurity fileSecurity = File.GetAccessControl(file);
            AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, true, typeof(NTAccount));
            foreach (FileSystemAccessRule rule in rules)
            {
                string name = rule.IdentityReference.Value;
                if (rule.FileSystemRights != FileSystemRights.FullControl)
                {
                    FileSecurity newFileSecurity = File.GetAccessControl(file);
                    FileSystemAccessRule newRule = new FileSystemAccessRule(name, FileSystemRights.Read, AccessControlType.Allow);
                    newFileSecurity.AddAccessRule(newRule);
                    File.SetAccessControl(file, newFileSecurity);
                }
            }
        }

        bool isMyUpPerm = false;
        // List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
        private void executableExcludePolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isMyUpPerm = true;
                var process = Process.Start("webroamransomwgui.exe");
                process.WaitForExit();
                isMyUpPerm = false;
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }




        public static AddBlacklist ad2b = null;
        public static AddWhiteList ad2w = null;
        public static AlertSettings als = null;
        private void addToblacklistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ad2b == null)
                {
                    ad2b = new AddBlacklist();
                    ad2b.Show();
                }
                else
                {
                    ad2b.Show();
                    ad2b.BringToFront();
                }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void addToWhitelistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ad2w == null)
                {
                    ad2w = new AddWhiteList();
                    ad2w.Show();
                }
                else
                {
                    ad2w.Show();
                    ad2w.BringToFront();
                }

            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] lines = File.ReadAllLines("..\\app_config.ini");
                //MessageBox.Show(String.Join(Environment.NewLine, lines));
                int i = 0;
                foreach (var l in lines)
                {
                    if (l.Contains("ProtectedExtensions"))
                    {
                        lines[i] = "ProtectedExtensions=" + textBox1.Text.Replace(Environment.NewLine, ",");
                        break;
                    }
                    i++;
                }
                File.Delete("..\\app_config.ini");
                File.WriteAllLines("..\\app_config.ini", lines, new UTF8Encoding(false));
                /*if (i == lines.Length)
                {
                    File.AppendAllLines("..\\app_config.ini", new string[] { "ProtectedExtensions=" + textBox1.Text.Replace(Environment.NewLine, ",") }, new UTF8Encoding(false));
                }*/
                button1.Enabled = false;
                Task.Factory.StartNew((Action)delegate ()
                {
                    /* try
                     {
                         Process p = Process.GetProcessesByName("WrArServ")[0];
                         FreeConsole();
                         if (AttachConsole((uint)p.Id))
                         {
                             //uint spid= p.SessionId;

                             // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                             SetConsoleCtrlHandler(null, true);

                             //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                             GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                             //
                             Thread.Sleep(2000);
                             FreeConsole();
                             SetConsoleCtrlHandler(null, false);
                         }
                     }
                     catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
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
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void disableAlertsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                disableAlertsToolStripMenuItem.Checked = !disableAlertsToolStripMenuItem.Checked;
                string[] lines = File.ReadAllLines("..\\app_config.ini");
                int i = 0;
                foreach (var l in lines)
                {
                    if (l.Contains("AlertBlock"))
                    {
                        lines[i] = "AlertBlock=" + (disableAlertsToolStripMenuItem.Checked ? "0" : "1");
                        break;
                    }
                    i++;
                }
                File.WriteAllLines("..\\app_config.ini", lines, new UTF8Encoding(false));
                if (i == lines.Length)
                {
                    File.AppendAllLines("..\\app_config.ini", new string[] { "AlertBlock=" + (disableAlertsToolStripMenuItem.Checked ? "0" : "1") }, new UTF8Encoding(false));
                }
                Task.Factory.StartNew((Action)delegate ()
                {
                    /*try
                    {
                        Process p = Process.GetProcessesByName("WrArServ")[0];
                        FreeConsole();
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
                });
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }


        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GenerateConsoleCtrlEvent(ConsoleCtrlEvent sigevent, int dwProcessGroupId);

        [DllImport("kernel32.dll")]
        static extern bool ProcessIdToSessionId(uint dwProcessId, out uint pSessionId);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        internal static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);
        internal delegate Boolean ConsoleCtrlDelegate(ConsoleCtrlEvent type);


        public enum ConsoleCtrlEvent
        {
            CTRL_C = 0,
            CTRL_BREAK = 1,
            CTRL_CLOSE = 2,
            CTRL_LOGOFF = 5,
            CTRL_SHUTDOWN = 6
        }
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!mediumToolStripMenuItem.Checked)
                {
                    mediumToolStripMenuItem.Checked = true;
                    mediumToolStripMenuItem.Font = new Font(mediumToolStripMenuItem.Font, FontStyle.Bold);
                    highToolStripMenuItem.Font = new Font(highToolStripMenuItem.Font, FontStyle.Regular);
                    disableToolStripMenuItem.Font = new Font(disableToolStripMenuItem.Font, FontStyle.Regular);
                    highToolStripMenuItem.Checked = disableToolStripMenuItem.Checked = false;
                    if (!File.Exists("..\\app_config.ini"))
                        return;
                    string[] lines = File.ReadAllLines("..\\app_config.ini", new UTF8Encoding(false));
                    int i = 0;
                    foreach (var l in lines)
                    {
                        if (l.Contains("SecurityLevel"))
                        {
                            lines[i] = "SecurityLevel=" + "2";
                            break;
                        }
                        i++;
                    }
                    
                    ProcessStartInfo ps1 = new ProcessStartInfo("fltmc.exe");
                    ps1.Arguments = "load WrArDriver";
                    ps1.CreateNoWindow = true;
                    ps1.WindowStyle = ProcessWindowStyle.Hidden;
                    ps1.UseShellExecute = false;
                    Process.Start(ps1);

                    timer3.Enabled = true;
                    timer3.Start();



                    File.WriteAllLines("..\\app_config.ini", lines, new UTF8Encoding(false));
                    if (i == lines.Length)
                    {
                        File.AppendAllLines("..\\app_config.ini", new string[] { "SecurityLevel=" + "2" }, new UTF8Encoding(false));
                    }
                    /*Task.Factory.StartNew((Action)delegate ()
                    {
                        try
                        {
                            FreeConsole();
                            Process[] p = Process.GetProcessesByName("WrArServ");
                            if (p.Length > 0 && AttachConsole((uint)p[0].Id))
                            {
                                //uint spid= p.SessionId;

                                // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                                SetConsoleCtrlHandler(null, true);

                                //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                                GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                                //
                                Thread.Sleep(2000);
                                FreeConsole();
                                SetConsoleCtrlHandler(null, false);
                            }
                        }
                        catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

                    });*/
                }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void highToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!highToolStripMenuItem.Checked)
                {
                    highToolStripMenuItem.Checked = true;
                    highToolStripMenuItem.Font = new Font(highToolStripMenuItem.Font, FontStyle.Bold);
                    mediumToolStripMenuItem.Font = new Font(mediumToolStripMenuItem.Font, FontStyle.Regular);
                    disableToolStripMenuItem.Font = new Font(disableToolStripMenuItem.Font, FontStyle.Regular);
                    mediumToolStripMenuItem.Checked = disableToolStripMenuItem.Checked = false;
                    string[] lines = File.ReadAllLines("..\\app_config.ini");
                    int i = 0;
                    foreach (var l in lines)
                    {
                        if (l.Contains("SecurityLevel"))
                        {
                            lines[i] = "SecurityLevel=" + "1";
                            break;
                        }
                        i++;
                    }
                    ProcessStartInfo ps1 = new ProcessStartInfo("fltmc.exe");
                    ps1.Arguments = "load WrArDriver";
                    ps1.CreateNoWindow = true;
                    ps1.WindowStyle = ProcessWindowStyle.Hidden;
                    ps1.UseShellExecute = false;
                    Process.Start(ps1).WaitForExit();

                    timer3.Enabled = true;
                    timer3.Start();

                    File.WriteAllLines("..\\app_config.ini", lines, new UTF8Encoding(false));
                    if (i == lines.Length)
                    {
                        File.AppendAllLines("..\\app_config.ini", new string[] { "SecurityLevel=" + "1" }, new UTF8Encoding(false));
                    }

                    /*  Task.Factory.StartNew((Action)delegate ()
                      {
                          try
                          {
                              FreeConsole();
                              Process[] p = Process.GetProcessesByName("WrArServ");
                              if (p.Length > 0 && AttachConsole((uint)p[0].Id))
                              {
                                  //uint spid= p.SessionId;

                                  // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                                  SetConsoleCtrlHandler(null, true);

                                  //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                                  GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                                  //
                                  Thread.Sleep(2000);
                                  FreeConsole();
                                  SetConsoleCtrlHandler(null, false);
                              }
                          }
                          catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

                      });*/
                }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!disableToolStripMenuItem.Checked)
                {
                    disableToolStripMenuItem.Checked = true;
                    disableToolStripMenuItem.Font = new Font(disableToolStripMenuItem.Font, FontStyle.Bold);
                    mediumToolStripMenuItem.Font = new Font(mediumToolStripMenuItem.Font, FontStyle.Regular);
                    highToolStripMenuItem.Font = new Font(highToolStripMenuItem.Font, FontStyle.Regular);
                    mediumToolStripMenuItem.Checked = highToolStripMenuItem.Checked = false;
                    string[] lines = File.ReadAllLines("..\\app_config.ini");
                    int i = 0;
                    foreach (var l in lines)
                    {
                        if (l.Contains("SecurityLevel"))
                        {
                            lines[i] = "SecurityLevel=" + "3";
                            break;
                        }
                        i++;
                    }
                    File.WriteAllLines("..\\app_config.ini", lines, new UTF8Encoding(false));
                    if (i == lines.Length)
                    {
                        File.AppendAllLines("..\\app_config.ini", new string[] { "SecurityLevel=" + "3" }, new UTF8Encoding(false));
                    }
                    timer3.Enabled = false;
                    timer3.Stop();
                    Process[] p = Process.GetProcessesByName("WrArServ");
                    if (p.Length > 0)// && AttachConsole((uint)p[0].Id))
                    {
                        p.ToList().ForEach((pa) => pa.Kill());
                    }
                    Process[] p2 = Process.GetProcessesByName("WrArEngine");
                    if (p2.Length > 0)// && AttachConsole((uint)p2[0].Id))
                    {
                        p2.ToList().ForEach((pa) => pa.Kill());
                    }
                  /*  ProcessStartInfo ps1 = new ProcessStartInfo("fltmc.exe");
                    ps1.Arguments = "unload WrArDriver";
                    ps1.CreateNoWindow = true;
                    ps1.WindowStyle = ProcessWindowStyle.Hidden;
                    ps1.UseShellExecute = false;
                    Process.Start(ps1);//.WaitForExit();
                    */
                    /*    Task.Factory.StartNew((Action)delegate ()
                        {
                            try
                            {
                                FreeConsole();
                                Process[] p = Process.GetProcessesByName("WrArServ");
                                if (p.Length > 0 && AttachConsole((uint)p[0].Id))
                                {
                                    //uint spid= p.SessionId;

                                    // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                                    SetConsoleCtrlHandler(null, true);

                                    //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                                    GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                                    //
                                    Thread.Sleep(2000);
                                    FreeConsole();
                                    SetConsoleCtrlHandler(null, false);
                                }
                            }
                            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

                        });*/
                }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }
        EnterPassword wnd = null;
        CreatePassword pwd = null;
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.ReadAllText("..\\app_config.ini", new UTF8Encoding(false)).Contains("Password"))
                {
                    if (pwd != null)
                    {
                        pwd.Focus();
                        return;
                    }
                    else
                    {
                        pwd = new CreatePassword();
                        if (pwd.ShowDialog() != DialogResult.OK)
                        {
                            pwd = null;
                            return;
                        }
                        pwd = null;
                    }
                }
                if (wnd != null)
                {
                    wnd.Focus();
                    return;
                }
                else
                {
                    wnd = new EnterPassword();
                    var text = (from s in File.ReadAllLines("..\\app_config.ini", new UTF8Encoding(false)) where s.Replace(" ", string.Empty).Contains("Password=") select s)?.DefaultIfEmpty();
                    string rs = text?.ElementAt(0).Replace(" ", string.Empty).Replace("Password=", "").Replace("=", "");
                    if (wnd.ShowDialog() != DialogResult.OK || Form1.CreateMD5(wnd.wrPaswd) != rs)
                    {
                        wnd = null;
                        return;
                    }
                    wnd = null;
                }

                toolStripMenuItem1.Checked = !toolStripMenuItem1.Checked;
                string[] lines = File.ReadAllLines("..\\app_config.ini");
                int i = 0;
                foreach (var l in lines)
                {
                    if (l.Contains("SelfProtection"))
                    {
                        lines[i] = "SelfProtection=" + (toolStripMenuItem1.Checked ? "1" : "0");
                        break;
                    }
                    i++;
                }
                File.WriteAllLines("..\\app_config.ini", lines, new UTF8Encoding(false));
                if (i == lines.Length)
                {
                    File.AppendAllLines("..\\app_config.ini", new string[] { toolStripMenuItem1.Checked ? "1" : "0" }, new UTF8Encoding(false));
                }
                /*  Task.Factory.StartNew((Action)delegate ()
                  {
                      try
                      {
                          FreeConsole();
                          Process[] p = Process.GetProcessesByName("WrArServ");
                          if (p.Length > 0 && AttachConsole((uint)p[0].Id))
                          {
                              //uint spid= p.SessionId;

                              // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                              SetConsoleCtrlHandler(null, true);

                              //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                              GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                              //
                              Thread.Sleep(2000);
                              FreeConsole();
                              SetConsoleCtrlHandler(null, false);
                          }
                      }
                      catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

                  });*/
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripMenuItem2.Checked = !toolStripMenuItem2.Checked;
                string[] lines = File.ReadAllLines("..\\app_config.ini");
                int i = 0;
                foreach (var l in lines)
                {
                    if (l.Contains("DisableConsoleInFormats"))
                    {
                        lines[i] = "DisableConsoleInFormats=" + (toolStripMenuItem2.Checked ? "1" : "0");
                        break;
                    }
                    i++;
                }
                File.WriteAllLines("..\\app_config.ini", lines, new UTF8Encoding(false));
                if (i == lines.Length)
                {
                    File.AppendAllLines("..\\app_config.ini", new string[] { "DisableConsoleInFormats=" + (toolStripMenuItem2.Checked ? "1" : "0") }, new UTF8Encoding(false));
                }
                /* Task.Factory.StartNew((Action)delegate ()
                  {
                      try
                      {
                          FreeConsole();
                          Process[] p = Process.GetProcessesByName("WrArServ");
                          if (p.Length > 0 && AttachConsole((uint)p[0].Id))
                          {
                              //uint spid= p.SessionId;

                              // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                              SetConsoleCtrlHandler(null, true);

                              //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                              GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                              //
                              Thread.Sleep(2000);
                              FreeConsole();
                              SetConsoleCtrlHandler(null, false);
                          }
                      }
                      catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

                  });*/
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            /* try
             {
                 Form1_Load(null, null);
             }
             catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
            */
        }

        private void browseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var currentDir = Environment.CurrentDirectory;

                new RestorePreviousVersions().Show();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                new bkupSetting().Show();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }
        bool alreadythere = true;
        public static int MaxbkupCount = 5;// * DriveInfo.GetDrives().Where(d=>d.DriveType == DriveType.Fixed && d.TotalFreeSpace > ((long)1024*1024*1024*2) && d.IsReady == true).Count();
        struct StruBkUp
        {
            public DateTime date;
            public string ID;
        }
        bool shouldGetBackUp(List<StruBkUp> datetms)
        {
            /*  const string fn0 = ".\\lsqdel.txt";
              //  int sleep = 0;
              string formt0 = "MM/dd/yyyy HH:mm tt";

              if (!File.Exists(fn0))
              {
                  File.WriteAllText(fn0, "0");
              }
              string last_read = File.ReadAllText(fn0).Trim();
              DateTime date1 = DateTime.Now;


              if (last_read == "0")
              {
                  File.Delete(fn0);
                  File.WriteAllText(fn0, DateTime.Now.ToString(formt0));               
                  return true;
              }
              else
              {
                  //  MessageBox.Show(date1.ToString()+Environment.NewLine+last_scan);
                  date1 = DateTime.ParseExact(last_read, formt0, CultureInfo.InvariantCulture);

              }*/
            // MessageBox.Show(MaxbkupCount.ToString());
            if (datetms.Count < MaxbkupCount)
            {
                return true;
            }
            if ((DateTime.Now - datetms[0].date).Hours / MaxbkupCount < (DateTime.Now - datetms[datetms.Count - 1].date).Hours)
            {
                var sdel = "delete shadows /shadow=" + datetms[0].ID + " /quiet";
                // 
                ProcessStartInfo ps1 = new ProcessStartInfo(Environment.SystemDirectory + "\\vssadmin.exe");
                ps1.Arguments = sdel;
                ps1.CreateNoWindow = true;
                ps1.WindowStyle = ProcessWindowStyle.Hidden;
                ps1.UseShellExecute = false;
                Process.Start(ps1).WaitForExit();
                //  Task.Run(()=>MessageBox.Show(ps1.FileName+" "+sdel));
                return true;
            }

            return false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {/*
                Task.Run(() =>
                {*/
                lock (bkupSetting.lk)
                {
                    bkupSetting.AllowStartVSS = true;
                }

                ProcessStartInfo ps1 = new ProcessStartInfo("sc.exe");
                ps1.Arguments = "config VSS start=demand";
                ps1.CreateNoWindow = true;
                ps1.WindowStyle = ProcessWindowStyle.Hidden;
             //   Process.Start(ps1).WaitForExit();

                ProcessStartInfo ps2 = new ProcessStartInfo("net");
                ps2.Arguments = "start VSS";
                ps2.CreateNoWindow = true;
                ps2.WindowStyle = ProcessWindowStyle.Hidden;
           //     Process.Start(ps2).WaitForExit();

                var drivlist = (from s in DriveInfo.GetDrives() where s.DriveType == DriveType.Fixed && s.IsReady == true && s.AvailableFreeSpace > ((long)bkupSetting.freespaceMin) select s)?.ToArray();
                //DriveInfo.GetDrives().GetType().
                // MessageBox.Show(drivlist[0]+Environment.NewLine+drivlist[1]);
                for (int i = 0; i < drivlist?.Length; i++)
                {
                    //  bool nobk = false;
                    ManagementObjectSearcher ms = new ManagementObjectSearcher("Select * from Win32_ShadowCopy");
                    List<StruBkUp> dttms = new List<StruBkUp>();
                    foreach (ManagementObject mo in ms.Get())
                    {
                        //    MessageBox.Show(drivlist[i].Name.Replace("\\", "") + "\"");
                        ManagementObjectSearcher disk = new ManagementObjectSearcher("Select * from Win32_Volume");
                        string mydrv = "";
                        foreach (var d in disk.Get())
                        {
                            if (d["DeviceID"].ToString() == mo["VolumeName"].ToString())
                            {
                                mydrv = d["DriveLetter"].ToString();
                                break;
                            }
                        }
                        //  MessageBox.Show(mo["VolumeName"].ToString() +Environment.NewLine+ disk["VolumeName"].ToString().ToLower
                        DateTime dt = ManagementDateTimeConverter.ToDateTime(mo["InstallDate"].ToString());
                        var st = new StruBkUp();
                        st.date = dt;
                        st.ID = mo["ID"].ToString();
                        if (mydrv == drivlist[i].Name.TrimEnd('\\'))
                            dttms.Add(st);
                        /*if(mo["VolumeName"].ToString().ToLower() == disk["VolumeName"].ToString().ToLower())
                        {

                            if (alreadythere)
                            {
                                nobk = true;
                            break;
                            }
                        }*/

                    }
                    /*if (nobk)
                            continue;*/
                    dttms = dttms.OrderBy(s => s.date).ToList();
                    if (!shouldGetBackUp(dttms))
                        continue;
                    // MessageBox.Show(drv);
                    // MessageBox.Show(drv);
                    //string command1 = $"$Action=new-scheduledtaskaction -execute \"{Environment.GetFolderPath(Environment.SpecialFolder.System)}\\Wbem\\WMIC.exe\" -Argument \"shadowcopy call create Volume = '{drv}' /nointeractive\"";
                    //  string command2 = $"$trigger = New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval (New-TimeSpan -Hours {numericUpDown1.Value})";
                    //string command3 = $"Register-ScheduledTask -TaskName ShadowCopy_WR_{drv[0].ToString()} -Trigger $Trigger -Action $Action -Description \"ShadowCopy_WR\"";

                    ManagementClass shadowCopy = new ManagementClass("Win32_ShadowCopy");



                    ManagementBaseObject inParams = shadowCopy.GetMethodParameters("Create");






                    int numItems = inParams.Properties.Count;







                    // Get an enumerator? And add some properties

                    IEnumerator eProps = inParams.Properties.GetEnumerator();






                    inParams.SetPropertyValue("Volume", (drivlist[i].Name));

                    inParams.SetPropertyValue("Context", "ClientAccessible");




                    /* for (int x = 0; x < numItems; x++)

                     {

                         eProps.MoveNext();

                         PropertyData pData = (PropertyData)eProps.Current;

                         Console.WriteLine("Name: " + pData.Name);

                         Console.WriteLine("Value: " + pData.Value);

                     }*/
                    try
                    {


                        ManagementBaseObject outParams = shadowCopy.InvokeMethod("Create", inParams, null);
                    }
                    catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
                    /*{
                        //MessageBox.Show(ex.Message);
                    }*/



                }

                string cmd15 = "net stop VSS";
                string cmd25 = "sc.exe config VSS start= disabled";
                var powershell5 = PowerShell.Create();
                //powershell.Commands.AddScript(command1);
                //powershell.Commands.AddScript(command2);
                powershell5.Commands.AddScript(cmd15);
            //    powershell5.Invoke();
                powershell5.Commands.AddScript(cmd25);
              //  powershell5.Invoke();
                lock (bkupSetting.lk)
                {
                    bkupSetting.AllowStartVSS = false;
                }

                /*    }).ContinueWith((t) =>
                    {
                     */
                lock (lk)
                {
                    runonce2 = true;
                }
                alreadythere = false;
                timer1.Start();
                //});
            }

            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void activiateNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //activate now
            var pr = new ProductKeyForm();
            pr.Show();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                string[] flines = File.ReadAllLines("..\\app_config.ini");
                string filecntnt = String.Join(Environment.NewLine, (from content in flines where !content.Contains("SecurityLevel") select content)?.DefaultIfEmpty());
                filecntnt += "\nSecurityLevel=3";
                File.WriteAllText("..\\app_config.ini", filecntnt, new UTF8Encoding(false));

                /*   Process p = Process.GetProcessesByName("WrArServ")[0];
                   FreeConsole();
                   if(AttachConsole((uint)p.Id)){
                   //uint spid= p.SessionId;

                   // MessageBox.Show(Process.GetProcessesByName("WRAREngine").Length.ToString() + Environment.NewLine + spid);
                   SetConsoleCtrlHandler(null, true);

                   //                            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, p.SessionId);
                   GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, 0);
                   //
                   Thread.Sleep(2000);
                   FreeConsole();
                   SetConsoleCtrlHandler(null, false);}*/
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

            return;
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                timer3.Enabled = false;
                timer3.Stop();
                
                    ProcessStartInfo psi1 = new ProcessStartInfo("sc.exe", "start wrardriver");
                    psi1.CreateNoWindow = true;
                    psi1.WindowStyle = ProcessWindowStyle.Hidden;
                    psi1.UseShellExecute = false;
                    psi1.Verb = "runas";
                    Process.Start(psi1).WaitForExit();
                    /*if (Process.GetProcessesByName("WrArServ").Length > 0)
                    {
                        Process.GetProcessesByName("WrArServ").ToList().ForEach(p => p.Kill());
                    }*/
                    var str = (new DirectoryInfo((AppDomain.CurrentDomain.BaseDirectory)).Parent.FullName);

                    try
                    {
                        // }
                        if (Process.GetProcessesByName("WrArServ").Length == 0)
                        {
                            var str1 = (new DirectoryInfo((AppDomain.CurrentDomain.BaseDirectory)).FullName);
                            // MessageBox.Show(str1);
                            ProcessStartInfo psi21 = new ProcessStartInfo(Path.Combine(str1, "WrArServ" + ".exe"));// "runas.exe");
                            psi21.CreateNoWindow = true;
                            // psi.Arguments = "/trustlevel:0x20000 \"" + Path.Combine(str, "WrArServ" + ".exe")+"\"";
                            psi21.WindowStyle = ProcessWindowStyle.Hidden;
                            psi21.UseShellExecute = true;
                            psi21.Verb = "runas";
                            Process.Start(psi21);
                            /*SystemUtility.ExecuteProcessUnElevated(Path.Combine(str, "WrArServ" + ".exe"), "");*/
                        }
                    var vp = Process.GetProcessesByName("WRAREngine");

                    if (vp == null || vp.Length == 0)
                    {
                        ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(str, "WRAREngine" + ".exe"));
                        psi.CreateNoWindow = true;
                        psi.WorkingDirectory = str;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.UseShellExecute = false;
                        psi.Verb = "runas";
                        vp[0] = Process.Start(psi);

                    
                    }
                    var s = $"-pid {vp[0].Id} -CriticalFlag " + (toolStripMenuItem1.Checked ? "1" : "0");
                    var psi11 = new ProcessStartInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\ProcessCritical\\ProcessCritical64.exe", s);
                    psi11.CreateNoWindow = true;
                    psi11.UseShellExecute = false;
                    psi11.WindowStyle = ProcessWindowStyle.Hidden;
                    psi11.WorkingDirectory = Environment.CurrentDirectory;
                    Task.Run(() => Process.Start(psi11)).Wait();
                }
                    catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

                
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

            
                //Thread.Sleep(4000);
              

                timer3.Enabled = true;
                timer3.Start();

            
        }
        void ProtectVSS(bool bpara)
        {
            toolStripMenuItem4.Checked = bpara;
            if (toolStripMenuItem4.Checked)
            {
                Task.Run(() =>
                {
                    string cmd1 = "net stop VSS";
                    string cmd2 = "sc.exe config VSS start= disabled";
                    var powershell = PowerShell.Create();
                    //powershell.Commands.AddScript(command1);
                    //powershell.Commands.AddScript(command2);
                    powershell.Commands.AddScript(cmd1);
                    //           powershell.Invoke();
                    powershell.Commands.AddScript(cmd2);
                    //             powershell.Invoke();
                });
                lock (bkupSetting.lk)
                {
                    bkupSetting.AllowStartVSS = false;
                }
            }
            /*else
                        Task.Run(() =>
                {
                    lock (bkupSetting.lk)
                    {
                        bkupSetting.AllowStartVSS = true;
                    }

                    ProcessStartInfo ps1 = new ProcessStartInfo("sc.exe");
                    ps1.Arguments = "config VSS start=demand";
                    ps1.CreateNoWindow = true;
                    ps1.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(ps1).WaitForExit();

                    ProcessStartInfo ps2 = new ProcessStartInfo("net");
                    ps2.Arguments = "start VSS";
                    ps2.CreateNoWindow = true;
                    ps2.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(ps2).WaitForExit();
                });*/
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItem4.Checked)
            {
                CaptchaDlg cdlg = new CaptchaDlg();
                if (cdlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
            ProtectVSS(!toolStripMenuItem4.Checked);
        }

        private void restoreAllSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy("alerts.conf.default", "alerts.conf", true);
                File.Copy("tmsetting.default", "tmsetting", true);
                File.Copy("bcsetting.default", "bcsetting", true);
                File.Copy("frssetting.default", "frssetting", true);
                File.Copy("..\\app_config.ini.default", "..\\app_config.ini", true);
                loadMenue();
                ProtectVSS(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Environment.CurrentDirectory + "\\WrAR_Report.txt.wrdb"))
                    Process.Start("notepad.exe", Environment.CurrentDirectory + "\\WrAR_Report.txt.wrdb");
            }
            catch
            { }
        }

        private void label2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
    public static class wrUpdate
    {
        private static void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            try
            {
                if (args != null)
                {
                    if (args.IsUpdateAvailable)
                    {
                        if (args.Mandatory)
                        {
                        }
                        else
                        {
                            if (MessageBox.Show($@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. Do you want to update the application now?", @"Update Available",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                            {
                                timer.Stop();
                                return;
                            }
                        }
                        AutoUpdater.Mandatory = true;
                        AutoUpdater.UpdateMode = Mode.Forced;

                        if (AutoUpdater.DownloadUpdate())
                        {
                            //ProcessProtector.isOn = false;
                            System.Environment.Exit(1);
                        }
                    }
                }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }
        static System.Timers.Timer timer;
        public static void Update()
        {
            try
            {
                if (!Form1.IsLicenseValid && (DateTime.Now.Month > 11 && DateTime.Now.Year > 2022))
                    return;
                timer = new System.Timers.Timer(100);
                AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
                AutoUpdater.DownloadPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                BasicAuthentication basicAuthentication = new BasicAuthentication("wrupdat1av", "Mv5T@fa.Up1");
                AutoUpdater.BasicAuthXML = AutoUpdater.BasicAuthDownload = basicAuthentication;
                timer.Elapsed += delegate
                {
                    AutoUpdater.Start(File.ReadAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\update.txt").Trim().Replace("[product]", Assembly.GetExecutingAssembly().FullName));
                    // timer.Stop();
                    timer.Interval = 16 * 60 * 1000;
                };
                timer.Start();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }
    }
    public static class AccessControl
    {
        public static void AddFileSecurity(string fileName, string account,
              FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the
            // current security settings.
            if (Directory.Exists(fileName))
            {
                var files = Directory.EnumerateFiles(fileName);

                foreach (var f in files)
                {
                    FileSecurity fSecurity = File.GetAccessControl(f);

                    // Add the FileSystemAccessRule to the security settings.
                    fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                        rights, controlType));

                    // Set the new access settings.
                    File.SetAccessControl(f, fSecurity);
                }
            }
            else if (File.Exists(fileName))
            {
                FileSecurity fSecurity = File.GetAccessControl(fileName);
                fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                        rights, controlType));
                File.SetAccessControl(fileName, fSecurity);
            }
            /*  var myDirectoryInfo = new DirectoryInfo(fileName);
              var myDirectorySecurity = myDirectoryInfo.GetAccessControl();
              myDirectorySecurity.AddAccessRule(new FileSystemAccessRule(account, rights, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, controlType));
              myDirectorySecurity.AddAccessRule(new FileSystemAccessRule(account, rights, InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, controlType));
              myDirectoryInfo.SetAccessControl(myDirectorySecurity);*/
        }

        // Removes an ACL entry on the specified file for the specified account.
        public static void RemoveFileSecurityALL(string fileName, bool asAdmin)/*, string account,
            FileSystemRights rights, AccessControlType controlType)*/
        {
            try
            {
                // MessageBox.Show(fileName, asAdmin.ToString());
                // Get a FileSecurity object that represents the
                // current security settings.
                if (Directory.Exists(fileName))
                {


                    // MessageBox.Show(fileName+Environment.NewLine+ Environment.UserDomainName + "\\" + Environment.UserName + Environment.NewLine+asAdmin);
                    //        Directory.GetAccessControl(fileName).SetAccessRuleProtection(true, false);
                    DirectorySecurity ds = Directory.GetAccessControl(fileName);// new DirectorySecurity();
                                                                                //      ds.SetAccessRuleProtection(true, false);
                    ds.RemoveAccessRuleAll(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, !asAdmin ? FileSystemRights.ReadAndExecute : FileSystemRights.FullControl, AccessControlType.Allow));
                    //       ds.RemoveAccessRuleAll(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                    //    ds.RemoveAccessRuleAll(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));

                    //ds.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                    //ds.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                    ds.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                    ds.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                    ds.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                    ds.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));

                    // Directory.SetAccessControl(fileName, ds);
                    //  ds.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, asAdmin ? FileSystemRights.ReadAndExecute : FileSystemRights.FullControl, AccessControlType.Allow));
                    ds.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, asAdmin ? FileSystemRights.ReadAndExecute : FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                    ds.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, asAdmin ? FileSystemRights.ReadAndExecute : FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                    Directory.SetAccessControl(fileName, ds);
                    // ds.SetAccessRuleProtection(true, false);
                    //  Directory.GetAccessControl(fileName).SetAccessRuleProtection(true, false);
                    //   var dSecurity = ds;
                    /*    dSecurity.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dSecurity.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dSecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + "Administrators", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dSecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + "Administrators", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dSecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dSecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        */
                    //Directory.SetAccessControl(fileName, new DirectorySecurity());
                    var files = Directory.EnumerateFiles(fileName);
                    foreach (var f in files)
                    {
                        FileSecurity fSecurity = new FileSecurity();/*File.GetAccessControl(f);
                        var collection = fSecurity.GetAccessRules(false, true, typeof(System.Security.Principal.NTAccount));
                        // Remove the FileSystemAccessRule from the security settings.
                        foreach (FileSystemAccessRule cc in collection)
                        {

                            fSecurity.SetAccessRuleProtection(true, false);
                            fSecurity.RemoveAccessRuleAll(cc);

                        }*/
                        //  fSecurity.SetAccessRuleProtection(false, false);
                        //  fSecurity.SetAccessRuleProtection(true, false);
                        fSecurity.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                        fSecurity.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                        /*fSecurity.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                        fSecurity.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                        fSecurity.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                        fSecurity.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                        */
                        fSecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, asAdmin ? FileSystemRights.ReadAndExecute : FileSystemRights.FullControl, AccessControlType.Allow));
                        fSecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, asAdmin ? FileSystemRights.ReadAndExecute : FileSystemRights.FullControl, AccessControlType.Allow));

                        /* fSecurity.RemoveAccessRuleAll(new FileSystemAccessRule(account,
                             rights, controlType));      */
                        // Set the new access settings.
                        /*  File.GetAccessControl(f).SetAccessRuleProtection(true, false);
                          File.GetAccessControl(f).RemoveAccessRuleAll(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                          File.GetAccessControl(f).RemoveAccessRuleAll(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                          File.GetAccessControl(f).RemoveAccessRuleAll(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.FullControl, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow));*/
                        try
                        {
                            File.SetAccessControl(f, fSecurity);
                        }
                        catch
                        {

                        }
                        //   MessageBox.Show(f);
                    }
                    /* var fsecMe = new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.ReadAndExecute, AccessControlType.Allow);
                     //MessageBox.Show(fileName);
                     var dsec = new DirectorySecurity(fileName, AccessControlSections.Access);
                     dsec.PurgeAccessRules(fsecMe.IdentityReference);
                     Directory.SetAccessControl(fileName, dsec);*/
                }
                else
                {
                    FileSecurity fSecurity = File.GetAccessControl(fileName);
                    /*  var collection = fSecurity.GetAccessRules(false, false, typeof(System.Security.Principal.NTAccount));
                      // Remove the FileSystemAccessRule from the security settings.
                      foreach (FileSystemAccessRule cc in collection)
                      {

                          //    fSecurity.SetAccessRuleProtection(false, false);
                              //fSecurity.RemoveAccessRuleSpecific(cc);

                      }*/
                    /*var fsecMe = new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.ReadAndExecute, AccessControlType.Allow);
                    //MessageBox.Show(fileName);
                    var dsec = new DirectorySecurity(fileName, AccessControlSections.Access);
                    dsec.PurgeAccessRules(fsecMe.IdentityReference);
                    Directory.SetAccessControl(fileName, dsec);*/
                    fSecurity.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                    fSecurity.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                    /* fSecurity.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                     fSecurity.AddAccessRule(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                     fSecurity.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                     fSecurity.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));*/
                    // fSecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.ReadAndExecute, AccessControlType.Allow));
                    fSecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, asAdmin ? FileSystemRights.ReadAndExecute : FileSystemRights.FullControl, AccessControlType.Allow));//, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                    //fSecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, asAdmin ? FileSystemRights.ReadAndExecute : FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                    /* fSecurity.RemoveAccessRuleAll(new FileSystemAccessRule(account,
                         rights, controlType));      */
                    // Set the new access settings.
                    try
                    {
                        File.SetAccessControl(fileName, fSecurity);
                    }
                    catch
                    {
                    }
                }

            }
            catch (Exception ex)
            {
                  MessageBox.Show(fileName + Environment.NewLine + ex.ToString());
            }

        }



        public static void SetAccessFileDenyOrAllow(string file, bool asAdmin, FileSystemRights ef = FileSystemRights.Write, AccessControlType act = AccessControlType.Deny)
        {
            //AccessControl.AddFileSecurity(file, System.Security.Principal.WindowsIdentity.GetCurrent().Name, ef, act);
            // MessageBox.Show(System.Security.Principal.WindowsIdentity.GetCurrent().Groups[0]);
            AccessControl.RemoveFileSecurityALL(file, asAdmin);//, "SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow);
                                                               //AccessControl.RemoveFileSecurity(file, "Administrators", FileSystemRights.FullControl, AccessControlType.Allow);
                                                               //AccessControl.RemoveFileSecurity(file, Environment.UserDomainName + "\\" + Environment.UserName, System.Security.AccessControl.FileSystemRights.ReadAndExecute, System.Security.AccessControl.AccessControlType.Allow);



        }
    }


    public class Csv
    {
        public static DataTable DataSetGet(string filename, string separatorChar, out List<string> errors)
        {
            errors = new List<string>();
            var table = new DataTable("CSVTable");
            using (var sr = new StreamReader(filename, new UTF8Encoding(false)))
            {
                string line;
                var i = 0;
                while (sr.Peek() >= 0)
                {
                    try
                    {
                        line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        var values = line.Split(new[] { separatorChar }, StringSplitOptions.None);
                        var row = table.NewRow();
                        for (var colNum = 0; colNum < values.Length - 1; colNum++)
                        {
                            var value = values[colNum];
                            if (i == 0)
                            {
                                table.Columns.Add(value, typeof(String));
                            }
                            else
                            {
                                row[table.Columns[colNum]] = value;
                            }
                        }
                        if (i != 0) table.Rows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show(ex.Message);
                        errors.Add(ex.Message);
                    }
                    i++;
                }
            }
            return table;
        }
    }
    internal class SystemUtility
    {
        /// <summary>
        /// We are elevated and should launch the process unelevated. We can't create the
        /// process directly without it becoming elevated. So to workaround this, we have
        /// explorer do the process creation (explorer is typically running unelevated).
        /// </summary>
        internal static void ExecuteProcessUnElevated(string process, string args, string currentDirectory = "")
        {
            var shellWindows = (IShellWindows)new CShellWindows();

            // Get the desktop window
            object loc = CSIDL_Desktop;
            object unused = new object();
            int hwnd;
            var serviceProvider = (IServiceProvider)shellWindows.FindWindowSW(ref loc, ref unused, SWC_DESKTOP, out hwnd, SWFO_NEEDDISPATCH);

            // Get the shell browser
            var serviceGuid = SID_STopLevelBrowser;
            var interfaceGuid = typeof(IShellBrowser).GUID;
            var shellBrowser = (IShellBrowser)serviceProvider.QueryService(ref serviceGuid, ref interfaceGuid);

            // Get the shell dispatch
            var dispatch = typeof(IDispatch).GUID;
            var folderView = (IShellFolderViewDual)shellBrowser.QueryActiveShellView().GetItemObject(SVGIO_BACKGROUND, ref dispatch);
            var shellDispatch = (IShellDispatch2)folderView.Application;

            // Use the dispatch (which is unelevated) to launch the process for us
            shellDispatch.ShellExecute(process, args, currentDirectory, string.Empty, SW_HIDE);
        }

        /// <summary>
        /// Interop definitions
        /// </summary>
        private const int CSIDL_Desktop = 0;
        private const int SWC_DESKTOP = 8;
        private const int SWFO_NEEDDISPATCH = 1;
        private const int SW_SHOWNORMAL = 1;
        private const int SVGIO_BACKGROUND = 0;
        private const int SW_HIDE = 0;
        private readonly static Guid SID_STopLevelBrowser = new Guid("4C96BE40-915C-11CF-99D3-00AA004AE837");

        [ComImport]
        [Guid("9BA05972-F6A8-11CF-A442-00A0C90A8F39")]
        [ClassInterfaceAttribute(ClassInterfaceType.None)]
        private class CShellWindows
        {
        }

        [ComImport]
        [Guid("85CB6900-4D95-11CF-960C-0080C7F4EE85")]
        [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        private interface IShellWindows
        {
            [return: MarshalAs(UnmanagedType.IDispatch)]
            object FindWindowSW([MarshalAs(UnmanagedType.Struct)] ref object pvarloc, [MarshalAs(UnmanagedType.Struct)] ref object pvarlocRoot, int swClass, out int pHWND, int swfwOptions);
        }

        [ComImport]
        [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            object QueryService(ref Guid guidService, ref Guid riid);
        }

        [ComImport]
        [Guid("000214E2-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellBrowser
        {
            void VTableGap01(); // GetWindow
            void VTableGap02(); // ContextSensitiveHelp
            void VTableGap03(); // InsertMenusSB
            void VTableGap04(); // SetMenuSB
            void VTableGap05(); // RemoveMenusSB
            void VTableGap06(); // SetStatusTextSB
            void VTableGap07(); // EnableModelessSB
            void VTableGap08(); // TranslateAcceleratorSB
            void VTableGap09(); // BrowseObject
            void VTableGap10(); // GetViewStateStream
            void VTableGap11(); // GetControlWindow
            void VTableGap12(); // SendControlMsg
            IShellView QueryActiveShellView();
        }

        [ComImport]
        [Guid("000214E3-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellView
        {
            void VTableGap01(); // GetWindow
            void VTableGap02(); // ContextSensitiveHelp
            void VTableGap03(); // TranslateAcceleratorA
            void VTableGap04(); // EnableModeless
            void VTableGap05(); // UIActivate
            void VTableGap06(); // Refresh
            void VTableGap07(); // CreateViewWindow
            void VTableGap08(); // DestroyViewWindow
            void VTableGap09(); // GetCurrentInfo
            void VTableGap10(); // AddPropertySheetPages
            void VTableGap11(); // SaveViewState
            void VTableGap12(); // SelectItem

            [return: MarshalAs(UnmanagedType.Interface)]
            object GetItemObject(UInt32 aspectOfView, ref Guid riid);
        }

        [ComImport]
        [Guid("00020400-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        private interface IDispatch
        {
        }

        [ComImport]
        [Guid("E7A1AF80-4D96-11CF-960C-0080C7F4EE85")]
        [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        private interface IShellFolderViewDual
        {
            object Application { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
        }

        [ComImport]
        [Guid("A4C6892C-3BA9-11D2-9DEA-00C04FB16162")]
        [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        public interface IShellDispatch2
        {
            void ShellExecute([MarshalAs(UnmanagedType.BStr)] string File, [MarshalAs(UnmanagedType.Struct)] object vArgs, [MarshalAs(UnmanagedType.Struct)] object vDir, [MarshalAs(UnmanagedType.Struct)] object vOperation, [MarshalAs(UnmanagedType.Struct)] object vShow);
        }
    }
}
