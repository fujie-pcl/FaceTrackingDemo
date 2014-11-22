using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceTrackingDemo
{
    public partial class MainForm : Form
    {
        private VideoCameraCapture videoCameraCapture = null;

        public MainForm()
        {
            InitializeComponent();

            this.videoCameraCapture = new VideoCameraCapture();
            this.videoCameraCapture.VideoCameraFrameCaptured += 
                videoCameraCapture_VideoCameraFrameCaptured;

            this.videoCameraCapture.Start();
        }

        void videoCameraCapture_VideoCameraFrameCaptured(object sender, 
            VideoCameraFrameCapturedEventArgs e)
        {
            this.pictureBox.Image = 
            OpenCvSharp.Extensions.BitmapConverter.ToBitmap(
                e.Image, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            this.pictureBox.Invalidate();

            this.labelDateTime.Text = e.DateTime.ToString("HH:mm:ss.fff");
        }
    }
}
