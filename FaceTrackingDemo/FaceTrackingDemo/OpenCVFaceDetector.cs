using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Reflection;

namespace FaceTrackingDemo
{
    /// <summary>
    /// OpenCVのFaceTrackingを利用した顔領域の追跡
    /// </summary>
    public class OpenCVFaceDetector
    {
        public static readonly string defaultHaarCascadesFolderPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "haarcascades");

        public static readonly string defaultHaarCascadeFileName =
            "haarcascade_frontalface_alt2.xml";

        private CascadeClassifier cascadeClassifier = null;

        public OpenCVFaceDetector()
        {
            this.cascadeClassifier = new CascadeClassifier(
                Path.Combine(defaultHaarCascadesFolderPath, defaultHaarCascadeFileName));
        }

        public Rect[] Detect(Mat image)
        {
            if (image.Width < 30 || image.Height < 30)
                return new Rect[0];

            Mat grey = image;
            if (image.Channels() != 1)
            {
                grey = new Mat(image.Rows, image.Cols, MatType.CV_8UC1);
                Cv2.CvtColor(image, grey, OpenCvSharp.ColorConversion.RgbToGray);
            }

//            return this.cascadeClassifier.DetectMultiScale(
//                grey, 1.5, 3, HaarDetectionType.ScaleImage, new Size(30, 30));
            return this.cascadeClassifier.DetectMultiScale(
                grey, 1.08, 2, 
                HaarDetectionType.DoCannyPruning |
                HaarDetectionType.DoRoughSearch |
                HaarDetectionType.FindBiggestObject |
                HaarDetectionType.ScaleImage,
                new Size(30, 30));
        }
    }
}
