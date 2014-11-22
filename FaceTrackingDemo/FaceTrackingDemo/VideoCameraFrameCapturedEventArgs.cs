using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp.CPlusPlus;

namespace FaceTrackingDemo
{
    /// <summary>
    /// VideoCameraCaptureのVideoCameraFrameCapturedイベントの情報
    /// </summary>
    public class VideoCameraFrameCapturedEventArgs
    {
        /// <summary>
        /// キャプチャされた画像データ
        /// </summary>
        public Mat Image { get; private set; }

        /// <summary>
        /// キャプチャした時刻
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// 現在のFPS
        /// </summary>
        public float CurrentFps { get; private set; }

        /// <summary>
        /// コンストラクタ（画像のみ指定）
        /// </summary>
        /// <param name="image">キャプチャした画像</param>
        public VideoCameraFrameCapturedEventArgs(Mat image)
        {
            this.Image = image;
            this.DateTime = DateTime.Now;
            this.CurrentFps = 0.0F;
        }

        /// <summary>
        /// コンストラクタ（画像と時刻両方指定）
        /// </summary>
        /// <param name="image">画像</param>
        /// <param name="dateTime">時刻</param>
        public VideoCameraFrameCapturedEventArgs(Mat image, DateTime dateTime)
        {
            this.Image = image;
            this.DateTime = dateTime;
            this.CurrentFps = 0.0F;
        }

        /// <summary>
        /// コンストラクタ（画像と時刻、現在のFPSを指定）
        /// </summary>
        /// <param name="image">画像</param>
        /// <param name="dateTime">時刻</param>
        /// <param name="currentFps">現在のFPS</param>
        public VideoCameraFrameCapturedEventArgs(Mat image, DateTime dateTime, float currentFps)
        {
            this.Image = image;
            this.DateTime = dateTime;
            this.CurrentFps = currentFps;
        }
    }
}
