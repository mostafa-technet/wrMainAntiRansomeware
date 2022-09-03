using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class AlertSettings : Form
    {
        public static Form AForm;

        public AlertSettings()
        {
            InitializeComponent(); AForm = this;
        }

        private void AlertSettings_FormClosed(object sender, FormClosedEventArgs e)
        {

            try
            {
                AForm = this;
                Form1.als = null;
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
           
        }

        private void AlertSettings_Load(object sender, EventArgs e)
        {
            try
            {
                AForm = this;
                textBox1.Text = File.ReadAllText("alerts.conf")+Environment.NewLine;
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

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string[] lines = textBox1.Text.Trim('\r', '\n').Split('\n');
                int i = 1;
                foreach (var line in lines)
                {
                    try
                    {
                        if (Path.GetDirectoryName(line.Replace("\\*", "")) != null)
                        {
                            i++;
                            continue;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid path in line "+i);
                        break;
                    }
                }
                File.WriteAllLines("alerts.conf", lines);
                button1.Enabled = false;
                Task.Factory.StartNew((Action)delegate ()
                {
                    try
                    {
                        TcpClient client = new TcpClient("localhost", 2555);
                        var netstrm = client.GetStream();
                        string s = "UPDATECONF";
                        byte[] buffer = ASCIIEncoding.ASCII.GetBytes(s);

                        netstrm.Write(buffer, 0, buffer.Length);
                        netstrm.Flush();
                        client.Close();
                    }
                    catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
                    
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

        private void button2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
