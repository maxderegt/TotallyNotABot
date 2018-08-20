using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoutubeExplode.Models;

namespace TotallyNotABot.audio
{
    class Player
    {
        private Audio2 audio;
        private Source source;
        public Playlist Current;

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            private set
            {
                if (value)
                {
                    Console.WriteLine("True");
                }
                else
                {
                    Console.WriteLine("False");
                }
                _isPlaying = value;
            }
        }

        public Player()
        {
            audio = new Audio2();
            source = new Source();
            Current = new Playlist();
        }

        /// <summary>
        /// Add a song to the currently playing songs
        /// </summary>
        /// <param name="song"></param>
        public void Add(Song song)
        {
            Current.Add(song, false);
        }

        public void Add(int index)
        {
            if (HasSearch())
            {
                Add(source.SearchList[index]);
            }
        }

        public void Searched(IReadOnlyList<Video> videos)
        {
            source.Searched(videos);
        }

        public void Play()
        {
            if (!IsPlaying)
            {
                IsPlaying = true;
                Loop();
            }
        }

        public bool HasSearch()
        {
            return source.SearchList.Count > 0;
        }


        public void Stop()
        {
            if (IsPlaying)
            {
                try
                {
                    audio.ffmpeg.Kill();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                IsPlaying = false;
            }
        }

        private async void Loop()
        {
            while (IsPlaying)
            {
                Song song = Current.Next();
                if (song == null)
                {
                    // TODO: reached end of playlist, loop if set
                    Stop();
                    return;
                }

                await audio.StreamAudio(await source.DownloadSong(song));
            }
        }
    }
}
