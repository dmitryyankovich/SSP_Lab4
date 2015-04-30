using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interfaces;

namespace PictureViewer
{
    public partial class Form1 : Form
    {
        public IImageViewer ImageViewer;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            var h1 = new HttpChannel(0);
            ChannelServices.RegisterChannel(h1);
            Object remoteOb = RemotingServices.Connect(
            typeof(IImageViewer),
            "http://localhost:54321/ImageViewer");
            ImageViewer = remoteOb as IImageViewer;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                var buffer = ImageViewer.GetFile(textBox1.Text);
                if (buffer != null)
                {
                    var ms = new MemoryStream(buffer);
                    var returnImage = Image.FromStream(ms);
                    pictureBox1.Image = returnImage;
                }
                else
                {
                    MessageBox.Show("There is no image with this name");
                    pictureBox1.Image = null;
                }
        }
    }
}
