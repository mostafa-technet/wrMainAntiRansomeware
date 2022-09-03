using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class RestoreFilesBrowser : Form
    {
        public static Form AForm;

        public RestoreFilesBrowser()
        {
            InitializeComponent(); AForm = this;
        }

        private void RestoreFilesBrowser_Load(object sender, EventArgs e)
        {
            try
            {
                string t = this.Text;                
                webBrowser1.Navigate("C:\\shadowcopy");
                Cursor = Cursors.Arrow;
                //this.Text = t.Replace(" - Waiting", "");
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.GoBack();
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.GoForward();
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.Navigate("C:\\shadowcopy");
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
         
        }

        private void RestoreFilesBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Directory.Delete("C:\\shadowcopy");
            }             
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
         
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            try
            {
                this.Text = Uri.UnescapeDataString(webBrowser1.Url.AbsolutePath.Replace("file:///", "").Replace("/", "\\").Replace("C:\\shadowcopy", RestorePreviousVersions.sdrve));
            }
            catch(Exception em){ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);}
       
        }
        
        }
}
