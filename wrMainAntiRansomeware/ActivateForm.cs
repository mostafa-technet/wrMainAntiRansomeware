using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class ActivateForm : Form
    {
        public static Form thisForm = null;
        public ActivateForm()
        {
            InitializeComponent(); //AForm = this;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the license activation process?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
        }

        private void ActivateForm_Load(object sender, EventArgs e)
        {
            Task.Run(() => Thread.Sleep(2000)).ContinueWith((t) =>
            {
                try
                { 
                this.BeginInvoke((MethodInvoker)
                    delegate
                {
                    //cs1.Step2(ProductKeyForm.SessionOfLic, textBox1.Text);
                    this.Visible = false;
                    new RegisterInfoForm().Show();
                });
                }
                catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

            });
        }
    }
}
