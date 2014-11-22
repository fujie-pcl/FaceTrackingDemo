using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp.CPlusPlus;
using System.Threading;

namespace FaceTrackingDemo
{
    public class VideoCameraCapture
    {
        /// <summary>
        /// キャプチャが完了したときのイベント
        /// </summary>
        public event EventHandler<VideoCameraFrameCapturedEventArgs> VideoCameraFrameCaptured;

        /// <summary>
        /// 実際にキャプチャを行うオブジェクト
        /// </summary>
        private VideoCapture videoCapture = null;

        /// <summary>
        /// バックグラウンドタスクのキャンセルトークンソース
        /// </summary>
        private CancellationTokenSource cts = null;

        /// <summary>
        /// バックグラウンドでキャプチャを行うタスク
        /// </summary>
        private Task task = null;

        /// <summary>
        /// メインのタスクスケジューラ
        /// </summary>
        private TaskScheduler callerTaskScheduler = null;

        /// <summary>
        /// 顔検出器
        /// </summary>
        private OpenCVFaceDetector faceDetector = null;

        /// <summary>
        /// 現在のFPS
        /// </summary>
        private float currentFps;

        /// <summary>
        /// 最後にキャプチャされた時間
        /// </summary>
        private DateTime lastCapturedTime;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VideoCameraCapture()
        {
            this.callerTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            this.faceDetector = new OpenCVFaceDetector();
        }

        public void Start()
        {
            if (this.videoCapture != null) return;

            this.videoCapture = VideoCapture.FromCamera(0);

            this.cts = new CancellationTokenSource();

            var token = this.cts.Token;

            this.task = Task.Factory.StartNew(() => { this.Run(token); }, token);
        }

        public void Stop()
        {
            if (this.videoCapture == null) return;

            this.cts.Cancel();

            try
            {
                this.task.Wait();
            }
            catch (AggregateException e)
            {
                
            }

            this.task = null;

            this.videoCapture = null;
        }

        private void Run(CancellationToken ct)
        {
            this.lastCapturedTime = DateTime.Now;

            while (true)
            {
                ct.ThrowIfCancellationRequested();

                Mat image = new Mat();
                this.videoCapture.Read(image);

                if (image.Width < 30 || image.Height < 30)
                    continue;

                DateTime dateTime = DateTime.Now;

                Rect[] faces = this.faceDetector.Detect(image);

                if (faces.Length > 0)
                {
                    Mat subImage = image.SubMat(faces[0]);
                    Mat destImage = image.SubMat(new Rect(0, 0, 64, 64));
                    Cv2.Resize(subImage, destImage, new Size(64, 64));                    
                }

                foreach (Rect face in faces)
                {
                    image.Rectangle(face, new Scalar(0, 255, 0), 3);
                }

                TimeSpan ts = dateTime.Subtract(this.lastCapturedTime);
                this.currentFps = 1000.0F / (float)ts.TotalMilliseconds;
                this.lastCapturedTime = dateTime;

                if (this.VideoCameraFrameCaptured != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        this.VideoCameraFrameCaptured(this,
                            new VideoCameraFrameCapturedEventArgs(image, dateTime, this.currentFps));
                    }, CancellationToken.None, TaskCreationOptions.None, this.callerTaskScheduler);
                }
            }
        }
    }
}
