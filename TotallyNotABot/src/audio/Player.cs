using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TotallyNotABot.commands;
using YoutubeExplode.Models;

namespace TotallyNotABot.audio
{
    class Player
    {
        private Audio audio;
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
            audio = new Audio();
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

        public List<Song> Searched(IReadOnlyList<Video> videos)
        {
            return source.Searched(videos);
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

        public async void Next()
        {
            await Stop();
            IsPlaying = true;
            Play();
        }

        public async Task Stop()
        {
            await commands.Commands.Discord.UpdateStatusAsync(null);
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

        private async void Loop()
        {
            while (IsPlaying)
            {
                Song song = Current.Next();
                if (song == null)
                {
                    // TODO: reached end of playlist, loop if set
                    await Stop();
                    Current.Clear();
                    return;
                }

                DiscordGame test = new DiscordGame
                {
                    Name = song.Title,
                    Details = "",
                    State = "playing music"
                };
                await commands.Commands.Discord.UpdateStatusAsync(game: test);

                await audio.StreamAudio(await source.DownloadSong(song));
            }
        }
    }
}
