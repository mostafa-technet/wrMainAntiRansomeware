using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class CaptchaDlg : Form
    {
        public CaptchaDlg()
        {
            InitializeComponent();
            DialogResult = DialogResult.No;
        }
        Captcha cp = null;
        private void CaptchaDlg_Load(object sender, EventArgs e)
        {
          cp  = new Captcha();
            pictureBox1.Image = Image.FromStream(cp.ProcessRequest());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Enter the Captcha please.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox3.Text != cp.CaptchaValue)
            {
                MessageBox.Show("Captcha is wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}
