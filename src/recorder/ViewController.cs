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
        private UIImage recordImage;
        private UIImage stopImage;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            recordImage = new UIImage("art.scnassets/mic.png");
            stopImage = new UIImage("art.scnassets/stop.png");

            var btnPlay = UIButton.FromType(UIButtonType.System);
            btnPlay.Frame = new CGRect(20, 200, 280, 44);
            btnPlay.SetTitle("Play sound", UIControlState.Normal);

            btnPlay.TouchUpInside += (sender, e) => {
                PlaySounds.PlaySound("art.scnassets/message.wav");
                PlaySounds.PlaySound(audioFilePath);
            };

            View.AddSubview(btnPlay);

            btnAudioRecord = UIButton.FromType(UIButtonType.System);
            btnAudioRecord.Frame = new CGRect(20, 300, 96, 96);
            btnAudioRecord.SetImage(recordImage, UIControlState.Normal);

            btnAudioRecord.TouchUpInside += (sender, e) => {
                if (!isRecording)
                {
                    btnAudioRecord.SetImage(stopImage, UIControlState.Normal);
                    StartRecording();
                }
                else
                {
                    btnAudioRecord.SetImage(recordImage, UIControlState.Normal);
                    StopRecording();
                }
            };

            View.AddSubview(btnAudioRecord);

        }

        private void StopRecording() {
            audioRecorder.StopRecording();
            isRecording = false;
        }

        private void StartRecording()
        {
            if (!isRecording)
            {
                isRecording = true;
                audioFilePath = CreateOutputUrl();
                audioRecorder = new AudioRecorder();
                audioRecorder.StartRecording(audioFilePath);
                audioRecorder.Recorder.FinishedRecording += OnFinishedRecording;
            }
        }

        private void OnFinishedRecording(object sender, AVStatusEventArgs e)
        {
            btnAudioRecord.SetImage(recordImage, UIControlState.Normal);

            audioRecorder.Recorder.Dispose();
            audioRecorder.Recorder = null;
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