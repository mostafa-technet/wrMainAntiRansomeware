using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrMainAntiRansomeware
{
    public partial class ActivateWindow : Form
    {
        public static Form AForm;

        public ActivateWindow()
        {
            InitializeComponent(); AForm = this;
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
           // button3.BackColor = Color.Brown;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async Task FadeOut(Form o, int interval = 80)
        {
            //Object is fully visible. Fade it out
            while (o.Opacity > 0.0)
            {
                await Task.Delay(interval);
                o.Opacity -= 0.5;
            }
            o.Opacity = 0; //make fully invisible       
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await FadeOut(this);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://webroam.com/renew");
            //this.Close();
        }
        private async void FadeIn(Form o, int interval = 80)
        {
            //Object is not fully invisible. Fade it in
            while (o.Opacity < 1.0)
            {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
            o.Opacity = 1; //make fully visible       
        }
        private void ActivateWindow_Load(object sender, EventArgs e)
        {
            this.Opacity = 0.4;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Right-this.Width, Screen.PrimaryScreen.Bounds.Bottom-this.Height-45);
            FadeIn(this);
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://webroam.com/renew");
        }
    }
}
