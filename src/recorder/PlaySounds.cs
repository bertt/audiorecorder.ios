using System;
using AudioToolbox;
using AVFoundation;
using Foundation;

namespace recorder
{
    public static class PlaySounds
    {
        public static AVAudioPlayer player;


        public static void PlaySound(NSUrl file)
        {
            NSError err;
            var mp3 = AudioFile.Open(file, AudioFilePermission.Read);
            if (mp3 != null)
            {
                player = new AVAudioPlayer(file, "Song", out err);
                player.Volume = 1.0f;
                player.FinishedPlaying += delegate
                {
                    player = null;
                };
                var res = player.PrepareToPlay();
                var res1 = player.Play();
                Console.WriteLine($"Play result: {res1}");
            }

        }

        public static void PlaySound(string file)
        {
            // var songURL = new NSUrl(file);
            var songUrl = NSUrl.FromFilename(file);
            PlaySound(songUrl);
        }
    }
}
