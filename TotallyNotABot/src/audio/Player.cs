using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoutubeExplode.Models;
using Playlist = TotallyNotABot.PlayList.Playlist;

namespace TotallyNotABot.audio
{
    class Player
    {
        private Audio audio;
        public Source source;
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
        public bool Add(Song song)
        {
            Current.Add(song, false);
            return true;
        }

        public bool Add(int index)
        {
            if (HasSearch())
            {
                Add(source.SearchList[index]);
                return true;
            }

            return false;
        }

        public bool Add(string song)
        {
            if (HasSearch())
            {
                for (int i = 0; i < source.SearchList.Count; i++)
                {
                    Song current = source.SearchList[i];
                    if (current.Title.ToLower().IndexOf(song.ToLower()) != -1)
                    {
                        Add(current);
                        return true;
                    }
                }
            }

            return false;
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
                    Current.Clear();
                    if (Current.Name == null)
                    {
                        await Stop();
                        return;
                    }
                    else
                    {
                        Current.Restart();
                        song = Current.Next();
                        if (song == null)
                        {
                            await Stop();
                            return;
                        }
                    }
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
