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
        public bool IsPlaying { get; private set; }

        public Player()
        {
            Current = new Playlist();
        }

        /// <summary>
        /// Add a song to the currently playing songs
        /// </summary>
        /// <param name="song"></param>
        public void Add(Song song)
        {
            IsPlaying = false;
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

                audio.StreamAudio(await source.DownloadSong(song));
            }
        }
    }
}
