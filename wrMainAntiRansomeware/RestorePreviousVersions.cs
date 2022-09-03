using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class RestorePreviousVersions : Form
    {
        public static Form AForm;

        public RestorePreviousVersions()
        {
            InitializeComponent(); AForm = this;
        }
        ManagementObjectCollection queryCollection = null;
        private void RestorePreviousVersions_Load(object sender, EventArgs e)
        {
            var atx = " - Waiting...";
            try
            {
                

                this.Text += atx;
                listBox1.Enabled = false;
                listBox1.Items.Clear();
                Task.Run(() =>
                {

                    while (true)
                    {
                        lock (Form1.lk)
                        {
                            if (Form1.runonce2)
                                break;
                        }
                        Thread.Sleep(100);

                    }

                    lock (bkupSetting.lk)
                    {
                        bkupSetting.AllowStartVSS = true;
                    }

                    ProcessStartInfo ps1 = new ProcessStartInfo("sc.exe");
                    ps1.Arguments = "config VSS start=demand";
                    ps1.CreateNoWindow = true;
                    ps1.WindowStyle = ProcessWindowStyle.Hidden;
                  //  Process.Start(ps1).WaitForExit();

                    ProcessStartInfo ps2 = new ProcessStartInfo("net");
                    ps2.Arguments = "start VSS";
                    ps2.CreateNoWindow = true;
                    ps2.WindowStyle = ProcessWindowStyle.Hidden;
                   // Process.Start(ps2).WaitForExit();

                //create a management scope object
                ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\cimv2");

                //create object query
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_ShadowCopy");

                //create object searcher
                ManagementObjectSearcher searcher =
                                            new ManagementObjectSearcher(scope, query);

                //get a collection of WMI objects
                queryCollection = searcher.Get();

                //enumerate the collection.
                foreach (ManagementObject m in queryCollection)
                    {
                        string fulldate = m["InstallDate"].ToString();
                    // MessageBox.Show(fulldate);
                    // access properties of the WMI object
                    listBox1.BeginInvoke(new MethodInvoker(delegate
                        {
                            listBox1.Items.Insert(0, $"{fulldate.Substring(0, 4)}/{fulldate.Substring(4, 2)}/{fulldate.Substring(6, 2)} {fulldate.Substring(8, 2)}:{fulldate.Substring(10, 2)}:{fulldate.Substring(12, 2)}");
                        }));

                    }
                    string cmd1 = "net stop VSS";
                    string cmd2 = "sc.exe config VSS start= disabled";
                    var powershell = PowerShell.Create();
                //powershell.Commands.AddScript(command1);
                //powershell.Commands.AddScript(command2);
                powershell.Commands.AddScript(cmd1);
                  //  powershell.Invoke();
                    powershell.Commands.AddScript(cmd2);
                    //powershell.Invoke();
                    lock (bkupSetting.lk)
                    {
                        bkupSetting.AllowStartVSS = false;
                    }
                }).ContinueWith((t) => this.BeginInvoke(new MethodInvoker(() => { panel1.Enabled = true; listBox1.Enabled = true; })));
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
            finally
            {
                this.Text = this.Text.Replace(atx, "");
            }
        
        }
        string mountpath;
        public static string sdrve = "";
       
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {    
                if (listBox1.Enabled && listBox1.SelectedIndex >= 0)
                {
                    lock (bkupSetting.lk)
                    {
                        bkupSetting.AllowStartVSS = true;
                    }
                    if (queryCollection != null)
                    {
                        int i = listBox1.Items.Count - 1;
                        foreach (ManagementObject m in queryCollection)
                        {
                            if (i == listBox1.SelectedIndex)
                            {
                                label2.Text = "ID: " + m["ID"] + Environment.NewLine + Environment.NewLine;

                                ManagementObjectSearcher ms = new ManagementObjectSearcher("Select * from Win32_Volume");
                                foreach (ManagementObject mo in ms.Get())
                                {
                                    var guid = mo["DeviceID"].ToString();

                                    if (guid == m["VolumeName"].ToString())
                                    {
                                        label2.Text += "Drive: " + mo["DriveLetter"] + Environment.NewLine + Environment.NewLine;
                                        sdrve = mo["DriveLetter"].ToString();
                                        break;
                                    }
                                }
                                label2.Text += "Date: " + listBox1.SelectedItem.ToString() + Environment.NewLine + Environment.NewLine;
                                label2.Text += "Machine: " + m["OriginatingMachine"].ToString() + Environment.NewLine + Environment.NewLine;
                                mountpath = m["DeviceObject"].ToString() + "\\";
                                break;
                            }
                            i--;
                        }
                    }
                    lock (bkupSetting.lk)
                    {
                        bkupSetting.AllowStartVSS = false;
                    }
                }
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (label2.Text == "Click on a backup from the List" || listBox1.Items.Count == 0)
                    return;
                if (listBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a backup item from the list!");
                    return;
                }
                if (MessageBox.Show("Are you sure that you want to delete the selected item?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
                string atx = " - Waiting...";
                this.Text += atx;
                button1.Enabled = false;
                listBox1.Enabled = false;
                var t = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        lock (bkupSetting.lk)
                        {
                            bkupSetting.AllowStartVSS = true;
                        }
                    /*              string command1 = "sc.exe config VSS start=demand";
                                  string command2 = "net start VSS";
                                  */
                        ProcessStartInfo ps1 = new ProcessStartInfo("sc.exe");
                        ps1.Arguments = "config VSS start=demand";
                        ps1.CreateNoWindow = true;
                        ps1.WindowStyle = ProcessWindowStyle.Hidden;
                        //Process.Start(ps1).WaitForExit();

                        ProcessStartInfo ps2 = new ProcessStartInfo("net");
                        ps2.Arguments = "start VSS";
                        ps2.CreateNoWindow = true;
                        ps2.WindowStyle = ProcessWindowStyle.Hidden;
                       // Process.Start(ps2).WaitForExit();

                        ProcessStartInfo ps23 = new ProcessStartInfo("cmd.exe");
                        ps23.Arguments = "/c vssadmin.exe delete shadows /shadow=" + label2.Text.Split('\n')[0].Replace("ID: ", "") + " /Quiet";
                        ps23.CreateNoWindow = true;
                        ps23.Verb = "runas";
                        ps23.WindowStyle = ProcessWindowStyle.Hidden;
                        Process.Start(ps23);
                      //  RestorePreviousVersions_Load(null, null);
                    /*          string cmd = "vssadmin.exe delete shadows /shadow=\"" + label2.Text.Split('\n')[0].Replace("ID: ", "") + "\" /Quiet";
                              var powershell0 = PowerShell.Create();
                              powershell0.Commands.AddScript(command1);
                              powershell0.Commands.AddScript(command2);
                              powershell0.Commands.AddScript(cmd);
                              powershell0.Invoke();

              */



                        lock (bkupSetting.lk)
                        {
                            bkupSetting.AllowStartVSS = false;
                        }
                    }
                    catch(Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }));
                t.Start();
                t.Join();
                string cmd1 = "net stop VSS";
                string cmd2 = "sc.exe config VSS start= disabled";
                var powershell = PowerShell.Create();
                //powershell.Commands.AddScript(command1);
                //powershell.Commands.AddScript(command2);
                powershell.Commands.AddScript(cmd1);
         //       powershell.Invoke();
                powershell.Commands.AddScript(cmd2);
           //     powershell.Invoke();
               

                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                queryCollection = null;
                listBox1.Items.Clear();
                RestorePreviousVersions_Load(null, null);
                this.Text = this.Text.Replace(atx, "");
                button1.Enabled = true;
                listBox1.Enabled = true;
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (label2.Text == "Click on a backup from the List" || listBox1.Items.Count == 0)
                    return;
                if(listBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a backup item from the list!");
                    return;
                }
                button2.Enabled = false;
                listBox1.Enabled = false;
                var atx = " - Waiting...";
                this.Text += atx;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    lock (bkupSetting.lk)
                    {
                        bkupSetting.AllowStartVSS = true;
                    }
                    /*              string command1 = "sc.exe config VSS start=demand";
                                  string command2 = "net start VSS";
                                  */
                    ProcessStartInfo ps1 = new ProcessStartInfo("sc.exe");
                    ps1.Arguments = "config VSS start=demand";
                    ps1.CreateNoWindow = true;
                    ps1.WindowStyle = ProcessWindowStyle.Hidden;
                    //Process.Start(ps1).WaitForExit();

                    ProcessStartInfo ps2 = new ProcessStartInfo("net");
                    ps2.Arguments = "start VSS";
                    ps2.CreateNoWindow = true;
                    ps2.WindowStyle = ProcessWindowStyle.Hidden;
                    //Process.Start(ps2).WaitForExit();
                    if (Directory.Exists("C:\\shadowcopy"))
                        Directory.Delete("C:\\shadowcopy");
                    ProcessStartInfo ps23 = new ProcessStartInfo("cmd.exe");
                    string foldershd = "C:\\shadowcopy";// + (new Random().Next(5800)+70).ToString();
                    ps23.Arguments = "/c mklink /d " + foldershd + " \"" + mountpath + "\"";
                    ps23.CreateNoWindow = true;
                    ps23.Verb = "runas";
                    ps23.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(ps23).WaitForExit();

                    lock (bkupSetting.lk)
                    {
                        bkupSetting.AllowStartVSS = false;
                    }
                    this.Cursor = Cursors.Arrow;
                    this.Text = this.Text.Replace(atx, "");
                    new RestoreFilesBrowser().Show();
                    /*          string cmd = "vssadmin.exe delete shadows /shadow=\"" + label2.Text.Split('\n')[0].Replace("ID: ", "") + "\" /Quiet";
                              var powershell0 = PowerShell.Create();
                              powershell0.Commands.AddScript(command1);
                              powershell0.Commands.AddScript(command2);
                              powershell0.Commands.AddScript(cmd);
                              powershell0.Invoke();

              */
                    string cmd1 = "net.exe stop VSS";
                    string cmd2 = "sc.exe config VSS start= disabled";
                    ProcessStartInfo ps24 = new ProcessStartInfo("cmd.exe");
                    ps24.Arguments = "/c " + cmd1;
                    ps24.CreateNoWindow = true;
                    ps24.Verb = "runas";
                    ps24.WindowStyle = ProcessWindowStyle.Hidden;
                   // Process.Start(ps24);
                    ProcessStartInfo ps25 = new ProcessStartInfo("cmd.exe");
                    ps25.Arguments = "/c " + cmd2;
                    ps25.CreateNoWindow = true;
                    ps25.Verb = "runas";
                    ps25.WindowStyle = ProcessWindowStyle.Hidden;
                 //   Process.Start(ps25);
                    /* var powershell = PowerShell.Create();
                     //powershell.Commands.AddScript(command1);
                     //powershell.Commands.AddScript(command2);
                     powershell.Commands.AddScript(cmd1);
                     powershell.Commands.AddScript(cmd2);
                     powershell.Invoke();*/


                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }


                button2.Enabled = true;
                listBox1.Enabled = true;
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
        
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if(listBox1.SelectedIndex!=-1)
                button2_Click(null, null);
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
            
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            listBox1_SelectedIndexChanged(null, null);
        }

        private void button3_Click(object sender, EventArgs e)
        { 
            var atx = " - Waiting...";
            try
            {
                if (listBox1.Items.Count == 0)
                    return;

                if (MessageBox.Show("Are you sure that you want to delete all backups?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
                
                if(!this.Text.Contains("Waiting"))
                this.Text += atx;
                button3.Enabled = false;
                listBox1.Enabled = false;
                var t = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        lock (bkupSetting.lk)
                        {
                            bkupSetting.AllowStartVSS = true;
                        }
                        /*              string command1 = "sc.exe config VSS start=demand";
                                      string command2 = "net start VSS";
                                      */
                        ProcessStartInfo ps1 = new ProcessStartInfo("sc.exe");
                        ps1.Arguments = "config VSS start=demand";
                        ps1.CreateNoWindow = true;
                        ps1.WindowStyle = ProcessWindowStyle.Hidden;
                    //    Process.Start(ps1).WaitForExit();

                        ProcessStartInfo ps2 = new ProcessStartInfo("net");
                        ps2.Arguments = "start VSS";
                        ps2.CreateNoWindow = true;
                        ps2.WindowStyle = ProcessWindowStyle.Hidden;
                      //  Process.Start(ps2).WaitForExit();

                        ProcessStartInfo ps23 = new ProcessStartInfo("vssadmin.exe");
                        ps23.Arguments = "delete shadows /All /Quiet";
                        ps23.CreateNoWindow = true;
                        ps23.Verb = "runas";
                        ps23.WindowStyle = ProcessWindowStyle.Hidden;
                        Process.Start(ps23).WaitForExit();
                        /*          string cmd = "vssadmin.exe delete shadows /shadow=\"" + label2.Text.Split('\n')[0].Replace("ID: ", "") + "\" /Quiet";
                                  var powershell0 = PowerShell.Create();
                                  powershell0.Commands.AddScript(command1);
                                  powershell0.Commands.AddScript(command2);
                                  powershell0.Commands.AddScript(cmd);
                                  powershell0.Invoke();

                  */



                        lock (bkupSetting.lk)
                        {
                            bkupSetting.AllowStartVSS = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }));
                t.Start();
                t.Join();  
                string cmd1 = "net stop VSS";
                string cmd2 = "sc.exe config VSS start= disabled";
                var powershell = PowerShell.Create();
                //powershell.Commands.AddScript(command1);
                //powershell.Commands.AddScript(command2);
                powershell.Commands.AddScript(cmd1);
              //  powershell.Invoke();
                powershell.Commands.AddScript(cmd2);
                //powershell.Invoke();


              //  listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                queryCollection = null;
              //  listBox1.Items.Clear();
                RestorePreviousVersions_Load(null, null);
               
            }
            catch (Exception em) { 
                ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); 
            
            }
            finally
            {
                this.Text = this.Text.Replace(atx, "");
                button3.Enabled = true;
                listBox1.Enabled = true;
                label2.Text = "";
            }

        }

        private void listBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
