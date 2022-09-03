using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    static class Program
    {
        static CancellationTokenSource cts;
        static TcpListener myTC;
        static Form form1glob;
        static void ListenOnPort()
        {
            int port = 2900;
            try
            {                

                try
                {
                    var tc = new TcpListener(IPAddress.Any, port);
                    tc.Start();
                    tc.Stop();

                }
                catch
                {
                    var tc = new TcpClient();
                    tc.Connect("127.0.0.1", port);
                    var strm = tc.GetStream();
                    string swi = Environment.GetCommandLineArgs().Length > 1 ? Environment.GetCommandLineArgs()[1] : "";
                    byte[] swibuf = ASCIIEncoding.ASCII.GetBytes(swi);
                    strm.Write(swibuf, 0, swibuf.Length);
                    strm.Flush();
                    tc.GetStream().Close();
                    tc.Close();
                    Environment.Exit(0);
                    return;
                }

                cts = new CancellationTokenSource();
                //Task.Factory.StartNew(() =>
                //{
                    myTC = new TcpListener(IPAddress.Any, port);
                    myTC.ExclusiveAddressUse = false;
                   myTC.Start();

                    // Socket s;
                    TcpClient client = null;
                    NetworkStream stream = null;
                    var ct = cts.Token;

                    ct.Register(() =>
                    {

                        if (myTC.Server.Connected)
                            myTC.Server.Disconnect(true);
                        myTC.Server.Dispose();
                        myTC.Stop();

                        Environment.Exit(0);
                    });
                    ct.ThrowIfCancellationRequested(); 
                
                    while (!ct.IsCancellationRequested)
                    {
                        
                        if (!myTC.Pending())
                        {
                            Thread.Sleep(300); // choose a number (in milliseconds) that makes sense
                            continue; // skip to next iteration of loop
                        }
                   
                        //s = myTC.AcceptSocket();
                        client = myTC.AcceptTcpClient();
                        stream = client.GetStream();
                        byte[] buf = new byte[1024];
                        if (client.Connected)
                        {
                            stream.Read(buf, 0, buf.Length);
                            string mystr = Encoding.ASCII.GetString(buf);
                            //
                            if (!String.IsNullOrWhiteSpace(mystr))
                            {
                                form1glob.BeginInvoke((MethodInvoker)delegate ()
                                {
                                 
                                   //     
                                     //   MessageBox.Show(mystr);
                                        var t = Type.GetType(mystr);

                            Form form = (Form)Type.GetType(mystr).GetField("AForm").GetValue(null);
                          //  if(mystr.Length>0)
                            //form = Application.OpenForms[mystr.Split('.')[1]];
                            if (form==null)
                                {
                               // MessageBox.Show(mystr);
                                //Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
                              
                                    form = (Form)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(mystr);
                                    form.Opacity = 100;
                                    //form.Activate();
                                    form.Show();
                                    Type.GetType(mystr).GetField("AForm").SetValue(null, form);
                                

                            }
                            form.Opacity = 100;
                            form.Show();
                                form.TopMost = true;
                                form.BringToFront();
                                form.TopMost = false;
                            
                                    
                               });

                            }
                        }
                        stream.Close();
                        client.Close();


                    }
                    Thread.CurrentThread.Abort();

              //  });
            
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        public static void runPrTask()
        {
           
                string queryString =
              "SELECT TargetInstance" +
              "  FROM __InstanceDeletionEvent " +
              "WITHIN  10 " +
              " WHERE TargetInstance ISA 'Win32_Process' " +
              "   AND TargetInstance.Name = '" + "WRAREngine.exe" + "'";

                // The dot in the scope means use the current machine
                string scope = @"\\.\root\CIMV2";

                // Create a watcher and listen for events
                ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
                watcher.Options.Timeout = TimeSpan.FromSeconds(5);
                watcher.EventArrived += delegate (object sender1, EventArrivedEventArgs e1)
                {
                   try
                    {
                        var str = (new DirectoryInfo(Path.GetDirectoryName(Environment.CurrentDirectory)).FullName);

                        //  MessageBox.Show(str);

                        ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(str, "WRAREngine" + ".exe"));
                        psi.CreateNoWindow = true;
                        psi.WorkingDirectory = str;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.UseShellExecute = false;
                        Process.Start(psi);
                    }
                    catch { }
                };
                Task.Run(() => watcher.Start());
                /*  Task.Run(() =>
                  {*/
                while (true)
                { try
            {
                    watcher.WaitForNextEvent();
                }
            catch { }
                }
                //}).Wait();
            
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
            if(Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length>1 && args.Length<1)
            {
                MessageBox.Show("Another instance of program is running!");
                Environment.Exit(0);
            }
                if (Process.GetProcessesByName("WRAREngine").Length > 0)
                {
                    Process.GetProcessesByName("WRAREngine").ToList().ForEach(p => p.Kill());
                }
                form1glob = new Form();
                form1glob.Opacity = 0;
                form1glob.Show();
                form1glob.Hide();
                Thread t = new Thread(new ThreadStart(ListenOnPort));
                t.Start();
                // string main = ".\\main.bat";
                wrUpdate.Update();

                /*if (File.Exists(main))
                {
                    Process.Start(main);
                    File.Delete(main);
                }*/
                // Thread tsk = new Thread(new ThreadStart(runPrTask));
                /*    var tsk = Task.Factory.StartNew(() =>
                {

                });*/
                //tsk.SetApartmentState(ApartmentState.STA);
                //tsk.Start();
                
                if (args.Length == 0)
            {
                Application.Run(new Form1());
            }
            t.Join();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //  runPrTask();
            //  tsk.Wait();

        }
    }
}
