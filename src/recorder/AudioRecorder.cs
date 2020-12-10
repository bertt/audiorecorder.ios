using AVFoundation;
using Foundation;

namespace recorder
{
    public class AudioRecorder
    {
        private AVAudioRecorder recorder;

        public void StartRecording(string url)
        {
            var session = AVAudioSession.SharedInstance();
            session.RequestRecordPermission((granted) =>
            {
                var options = AVAudioSessionCategoryOptions.DefaultToSpeaker;
                session.SetCategory(AVAudioSession.CategoryPlayAndRecord, options, out NSError error);
                session.SetActive(true, out error);
                PrepareAudioRecording(url); 
                recorder.Record();
            });
        }

        public void StopRecording()
        {
            recorder.Stop();
        }

        private void PrepareAudioRecording(string url)
        {
            var audioSettings = new AudioSettings
            {
                SampleRate = 44100,
                NumberChannels = 1,
                AudioQuality = AVAudioQuality.High,
                Format = AudioToolbox.AudioFormatType.MPEG4AAC,
            };

            // Set recorder parameters
            var url1 = NSUrl.FromFilename(url);
            recorder = AVAudioRecorder.Create(url1, audioSettings, out NSError error);
            recorder.RecordFor(10);

            // Set Recorder to Prepare To Record
            if (!recorder.PrepareToRecord())
            {
                recorder.Dispose();
                recorder = null;
            }
            else
            {
                recorder.FinishedRecording += OnFinishedRecording;
            }
        }


        private void OnFinishedRecording(object sender, AVStatusEventArgs e)
        {
            recorder.Dispose();
            recorder = null;
        }
    }
}