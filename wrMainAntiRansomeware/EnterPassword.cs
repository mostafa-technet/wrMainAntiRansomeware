using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class EnterPassword : Form
    {
        public string wrPaswd = "";
        public EnterPassword()
        {
            InitializeComponent(); //AForm = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string md5i = Form1.CreateMD5(textBox1.Text);
            var text = from s in File.ReadAllLines("..\\app_config.ini", new UTF8Encoding(false)) where s.Replace(" ", string.Empty).Contains("Password=") select s;
            string rs = text.ElementAt(0).Replace(" ", string.Empty).Replace("Password=", "");
            if (md5i != rs)
            {
                MessageBox.Show("Invalid Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }
            wrPaswd = textBox1.Text;
        //    MessageBox.Show("The password was saved successfully!");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void EnterPassword_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.button1_Click(null, null);
            }
        }
    }
}
