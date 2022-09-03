using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class LicenseDetailsForm : Form
    {
        public static Form AForm;

        public LicenseDetailsForm()
        {
            InitializeComponent(); AForm = this;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            { 
            if(ProductKeyForm.thisForm!=null)
            ProductKeyForm.thisForm.Close();
            this.Close();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }

        private void LicenseDetailsForm_Load(object sender, EventArgs e)
        {
            //ProductKeyForm.cs1.Step4(ProductKeyForm.SessionOfLic, "");
            try {
                label2.Text = UserInfoForm.LicFinished;
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            { 
            printDocument1.Print();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }

        private int linesPrinted;
        private string[] lines;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            { 
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;
            Brush brush = new SolidBrush(richTextBox1.ForeColor);

            while (linesPrinted < lines.Length)
            {
                e.Graphics.DrawString(lines[linesPrinted++],
                    richTextBox1.Font, brush, x, y);
                y += 15;
                if (y >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            linesPrinted = 0;
            e.HasMorePages = false;
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            { 
            char[] param = { '\n' };

                lines = richTextBox1.Text.Split(param);
           

            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }
        private bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "https://www.webroam.com/licenseAR.txt";
                WebClient wc = new WebClient();
                if (RemoteFileExists(url))
                    richTextBox1.Text = wc.DownloadString(url);
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }
    }
}
