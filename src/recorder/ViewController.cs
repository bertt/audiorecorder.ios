using AVFoundation;
using CoreGraphics;
using Foundation;
using System;
using System.IO;
using UIKit;

namespace recorder
{
    public partial class ViewController : UIViewController
    {
        private string audioFilePath;
        private UIButton btnAudioRecord;
        private bool isRecording;
        private AudioRecorder audioRecorder;


        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            var btnPlay = UIButton.FromType(UIButtonType.System);
            btnPlay.Frame = new CGRect(20, 200, 280, 44);
            btnPlay.SetTitle("Play sound", UIControlState.Normal);

            btnPlay.TouchUpInside += (sender, e) => {
                PlaySounds.PlaySound(audioFilePath);
            };

            View.AddSubview(btnPlay);


            btnAudioRecord = UIButton.FromType(UIButtonType.System);
            btnAudioRecord.Frame = new CGRect(20, 300, 280, 44);
            btnAudioRecord.SetTitle("start", UIControlState.Normal);

            btnAudioRecord.TouchUpInside += (sender, e) => {
                if (!isRecording)
                {
                    StartRecording();
                }
                else
                {
                    StopRecording();
                }
            };

            View.AddSubview(btnAudioRecord);

        }

        private void StopRecording() {
            audioRecorder.StopRecording();
            btnAudioRecord.SetTitle("start", UIControlState.Normal);
            isRecording = false;
        }

        private void StartRecording()
        {
            if (!isRecording)
            {
                Console.WriteLine("Begin Recording");
                isRecording = true;
                btnAudioRecord.SetTitle("stop", UIControlState.Normal);

                audioFilePath = CreateOutputUrl();
                audioRecorder = new AudioRecorder();
                audioRecorder.StartRecording(audioFilePath);
            }
        }

        private string CreateOutputUrl()
        {
            var fileName = $"Myfile-{DateTime.Now.ToString("yyyyMMddHHmmss")}.m4a";
            var tempRecording = Path.Combine(Path.GetTempPath(), fileName);

            return tempRecording;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}