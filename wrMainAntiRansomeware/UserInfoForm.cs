using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class UserInfoForm : Form
    {
        public static Form AForm;

        string[] RgText = File.ReadAllLines("clst.dat");
        public static Form thisForm = null;
        public static string LicFinished;
        public UserInfoForm()
        {
            InitializeComponent(); AForm = this;
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
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            try
            { 
            if (textBox1.Text.Trim() == ""|| textBox2.Text.Trim() == ""|| textBox3.Text.Trim() == ""|| textBox4.Text.Trim() == ""
                ||comboBox1.Text.Trim()=="" || comboBox2.Text.Trim() == "" || comboBox3.Text.Trim() == "")
            {
                MessageBox.Show("You must fill every field required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                    button2.Enabled = true;
                    return;
            }
            if(textBox2.Text!=textBox3.Text)
            {
                MessageBox.Show("Email and Confirm Email must be the same!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Focus();
                    button2.Enabled = true;
                    return;
            }
                if (!IsValidEmail(textBox3.Text))
                {
                    MessageBox.Show("Email format wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox3.Focus();
                    button2.Enabled = true;
                    return;
                }

                var macAddr =
  (
      from nic in NetworkInterface.GetAllNetworkInterfaces()
      where nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet  || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
      select nic.GetPhysicalAddress().ToString()
  ).FirstOrDefault();
                var licontent = Form1.CreateMD5(macAddr);
                string[] fileCn = new string[3];
                fileCn[1] = licontent;
                fileCn[0] = ProductKeyForm.License;

                File.WriteAllLines("license", fileCn);
                ProductKeyForm.cs1.Step3(ProductKeyForm.SessionOfLic, textBox1.Text.Trim()+","+ textBox2.Text.Trim() + "," +
                textBox4.Text.Trim() + "," 
                + comboBox1.Text.Trim() + "," + comboBox2.Text.Trim() + "," + comboBox3.Text.Trim());
               /* MessageBox.Show(macAddr + "," + "1001" + "," +
               ProductKeyForm.license[2]);*/
                string rs1 = ProductKeyForm.cs1.Step4(ProductKeyForm.SessionOfLic, macAddr + "," + "1001" + "," +
               ProductKeyForm.license[2]);
                //MessageBox.Show(rs1);
                string[] result = rs1.Split(',');
                fileCn[2] = result[0];
                LicFinished = result.Last();
                File.WriteAllLines("license", fileCn);
            this.Visible = false;
            //  this.Close();
            new LicenseDetailsForm().Show();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                MessageBox.Show("Could not proceed to next step! Some error occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            try
            { 
            this.Visible = false;
            if (RegisterInfoForm.thisForm == null)
                new RegisterInfoForm().Show();
            else
                RegisterInfoForm.thisForm.Show();
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            try
            { 
            foreach(var s in RgText)
            {
                string[] tmp = s.Split(',');
                if(!comboBox1.Items.Contains(tmp[1]))
                {
                    comboBox1.Items.Add(tmp[1]);
                }
            }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
            foreach (var s in RgText)
            {
                string[] tmp = s.Split(',');
                if (comboBox1.Text == tmp[1] && !comboBox2.Items.Contains(tmp[2]))
                {
                    comboBox2.Items.Add(tmp[2]);
                }
            }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
            foreach (var s in RgText)
            {
                string[] tmp = s.Split(',');
                if (comboBox2.Text == tmp[2] && !comboBox3.Items.Contains(tmp[0]))
                {
                    comboBox3.Items.Add(tmp[0]);
                }
            }
            }
            catch (Exception em) { ProductKeyForm.FAppendAllText("wrlog2.txt.wrdb", new StackFrame(1, true).GetFileName() + " " + new StackFrame(1, true).GetFileLineNumber() + Environment.NewLine + em.ToString() + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine); }
            
        }
    }
}
