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
    public partial class RegisterInfoForm : Form
    {
        public static Form thisForm = null;
        public RegisterInfoForm()
        {
            InitializeComponent(); //AForm = this;
            try
            { 
            thisForm = this;
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            { 
            if (MessageBox.Show("Are you sure you want to close the license activation process?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            { 
            this.Visible = false;

           // ProductKeyForm.thisForm.Show();
            if (ProductKeyForm.thisForm == null)
                new ProductKeyForm().Show();
            else
                ProductKeyForm.thisForm.Show();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            try
            { 
            if (comboBox2.Text.Trim() == ""||comboBox1.Text.Trim()=="")
            {
                MessageBox.Show("You must fill every field required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox2.Focus();
                    button2.Enabled = true;
                    return;
            }

            int isc = ProductKeyForm.cs1.Step2(ProductKeyForm.SessionOfLic, comboBox2.SelectedIndex+","+textBox2.Text.Trim()+","+comboBox1.Text);
                //MessageBox.Show(isc.ToString());
                if(isc<0)
                {
                    MessageBox.Show("Some error occured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button2.Enabled = true;
                    return;
                }

            this.Visible = false;

            if(UserInfoForm.thisForm==null)
            new UserInfoForm().Show();
            else
                UserInfoForm.thisForm.Show();
                //this.Close();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                MessageBox.Show("Could not proceed to next step! Some error occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button2.Enabled = true;
        }

        private void RegisterInfoForm_Load(object sender, EventArgs e)
        {
            try
            { 
            comboBox2.SelectedIndex = 0;
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                MessageBox.Show("Could not proceed to next step! Some error occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
