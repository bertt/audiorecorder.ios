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
        private AVAudioRecorder recorder;
        private UIButton btnAudioRecord;
        private bool isRecording = false;

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
                    btnAudioRecord.SetTitle("stopping...", UIControlState.Normal);

                    StopRecording();
                }
            };

            View.AddSubview(btnAudioRecord);

        }

        private void StopRecording()
        {
            recorder.Stop();
        }

        private void StartRecording()
        {
            if (!isRecording)
            {
                Console.WriteLine("Begin Recording");
                isRecording = true;
                btnAudioRecord.SetTitle("stop", UIControlState.Normal);

                var session = AVAudioSession.SharedInstance();
                session.RequestRecordPermission((granted) =>
                {
                    Console.WriteLine($"Audio Permission: {granted}");

                    if (granted)
                    {
                        //var options = new
                        var options = AVAudioSessionCategoryOptions.DefaultToSpeaker;
                        session.SetCategory(AVAudioSession.CategoryPlayAndRecord, options, out NSError error);
                        if (error == null)
                        {
                            session.SetActive(true, out error);
                            if (error != null)
                            {
                            }
                            else
                            {
                                var isPrepared = PrepareAudioRecording() && recorder.Record();
                            }
                        }
                        else
                        {
                            Console.WriteLine(error.LocalizedDescription);
                        }
                    }
                    else
                    {
                        Console.WriteLine("YOU MUST ENABLE MICROPHONE PERMISSION");
                    }
                });

            }
        }


        private bool PrepareAudioRecording()
        {
            var result = false;

            audioFilePath = CreateOutputUrl();

            Console.WriteLine($"audio file: {audioFilePath}");

            var audioSettings = new AudioSettings
            {
                SampleRate = 44100,
                NumberChannels = 1,
                AudioQuality = AVAudioQuality.High,
                Format = AudioToolbox.AudioFormatType.MPEG4AAC,
            };

            // Set recorder parameters
            var url = NSUrl.FromFilename(audioFilePath);
            recorder = AVAudioRecorder.Create(url, audioSettings, out NSError error);
            recorder.RecordFor(10);

            if (error == null)
            {
                // Set Recorder to Prepare To Record
                if (!recorder.PrepareToRecord())
                {
                    recorder.Dispose();
                    recorder = null;
                }
                else
                {
                    recorder.FinishedRecording += OnFinishedRecording;
                    result = true;
                }
            }
            else
            {
                Console.WriteLine(error.LocalizedDescription);
            }

            return result;
        }

        private void OnFinishedRecording(object sender, AVStatusEventArgs e)
        {
            btnAudioRecord.SetTitle("start", UIControlState.Normal);
            isRecording = false;
            recorder.Dispose();
            recorder = null;

            Console.WriteLine($"Done Recording (status: {e.Status})");
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