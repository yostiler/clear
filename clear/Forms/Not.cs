using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clear.Forms
{
    public partial class Not : Form
    {
        int toastX, toastY;
        public Not(string type, string message)
        {
            InitializeComponent();
            label1.Text = type;
            label2.Text = message;
            switch (type)
            {
                case "SUCCESS":
                    guna2Panel1.BackColor = Color.FromArgb(57, 155, 53);
                    guna2PictureBox1.Image = Properties.Resources.s2;
                    break;
                case "ERROR":
                    guna2Panel1.BackColor = Color.FromArgb(227, 50, 45);
                    guna2PictureBox1.Image = Properties.Resources.e2;
                    break;
            }
        }

        private void Not_Load(object sender, EventArgs e)
        {
            Position();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toastY += 5;
            this.Location = new Point(toastX, toastY);

            if (toastY >= Screen.PrimaryScreen.WorkingArea.Height - this.Height - 835)
            {
                timer1.Stop();
                Task.Delay(2000).ContinueWith(t => timer2.Start(), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        int y = 100;

        private void timer2_Tick(object sender, EventArgs e)
        {
            toastY -= 5;
            this.Location = new Point(toastX, toastY);

            if (this.Top + this.Height < 0)
            {
                timer2.Stop();
                this.Close();
            }
        }

        private void Position()
        {
            int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            toastX = ScreenWidth - this.Width - 10;
            toastY = -this.Height;

            this.Location = new Point(toastX, toastY);
        }
    }
}