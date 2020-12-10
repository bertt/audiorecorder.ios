using AVFoundation;
using Foundation;

namespace recorder
{
    public class AudioRecorder
    {
        public AVAudioRecorder Recorder;

        public void StartRecording(string url)
        {
            var session = AVAudioSession.SharedInstance();
            session.RequestRecordPermission((granted) =>
            {
                var options = AVAudioSessionCategoryOptions.DefaultToSpeaker;
                session.SetCategory(AVAudioSession.CategoryPlayAndRecord, options, out NSError error);
                session.SetActive(true, out error);
                PrepareAudioRecording(url); 
                Recorder.Record();
            });
        }

        public void StopRecording()
        {
            Recorder.Stop();
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
            Recorder = AVAudioRecorder.Create(url1, audioSettings, out NSError error);
            Recorder.RecordFor(10);

            // Set Recorder to Prepare To Record
            //if (!Recorder.PrepareToRecord())
            //{
            //    Recorder.Dispose();
            //    Recorder = null;
            //}
            //else
            //{
            //    Recorder.FinishedRecording += OnFinishedRecording;
            //}
        }


    }
}