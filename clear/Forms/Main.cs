using clear.UCs;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using clear.Forms;

namespace clear
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            help1.Visible = false;
            home1.Visible = true;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Home homeControl = home1;

            if (homeControl != null)
            {
                homeControl.MyGradientPanel.Visible = !homeControl.MyGradientPanel.Visible;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            home1.Visible = false;
            help1.Visible = true;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string caminhoArquivo = @"C:\Users\token.exe";

            if (File.Exists(caminhoArquivo))
            {
                File.Delete(caminhoArquivo);
            }

            Application.Exit();
        }
    }
}