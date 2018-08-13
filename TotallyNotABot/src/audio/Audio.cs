using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DSharpPlus.Entities;
using TotallyNotABot.commands;
using VideoLibrary;
using Video = YoutubeExplode.Models.Video;

namespace TotallyNotABot.audio
{
    class Audio
    {
        public static readonly string VideoFile = "discordbot\\video.webm";
        // List tracking songs resulting from the search command
        public List<Song> SearchList;
        // Queue of songs currently playing
        public Playlist Current;

        public bool IsPlaying { get; set; }
        public bool Repeat { get; set; }

        public Process ffmpeg { get; set; }

        public Audio()
        {
            SearchList = new List<Song>();
            Current = new Playlist();
            Repeat = false;
        }

        /// <summary>
        /// Add a song to the currently playing songs
        /// </summary>
        /// <param name="song"></param>
        public void Add(Song song)
        {
            Current.Add(song, false);
        }

        /// <summary>
        /// Set the seachlist based on youtube search results
        /// </summary>
        /// <param name="videos"></param>
        public void Searched(IReadOnlyList<Video> videos)
        {
            this.SearchList.Clear();
            foreach (Video video in videos)
            {
                this.SearchList.Add(new Song(video));
            }
        }
        
        /// <summary>
        /// Start playing the current playlist
        /// </summary>
        public void Start()
        {
            if (ffmpeg == null || ffmpeg.HasExited)
            {
                Play();
            }
            else
            {
                Console.Error.WriteLine("ffmpeg is already running!");
            }
        }

        /// <summary>
        /// Continues loop running when playing
        /// </summary>
        /// <param name="isPlaying"></param>
        private void Play(bool isPlaying = true)
        {
            IsPlaying = isPlaying;

            while (IsPlaying)
            {
                // We can't play without songs.
                if (Commands.Connection == null || Current.Songs.Count <= 0) {
                    Stop();
                }

                // Get the next song and play it.
                Song song = Current.Next();
                if (song == null) continue;
                PlaySong(song);
                // TODO: not this! XD
                Current.Index = 0;
                if (!Repeat)
                {
                    Stop();
                }
            }
        }

        /// <summary>
        /// Download and play a single song
        /// </summary>
        /// <param name="song"></param>
        private void PlaySong(Song song)
        {
            // Download the song
            DownloadUrl(song.Url);
            // Set the discord status
            DiscordGame game = new DiscordGame
            {
                Name = song.Title,
                Details = "",
                State = "Playing music"
            };
            Commands.Discord.UpdateStatusAsync(game: game);

            try
            {
                PlayAudio();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Reset the discord status
            Commands.Discord.UpdateStatusAsync();
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public async void DownloadUrl(string url)
        {
            // starting point for YouTube actions
            YouTube youTube = YouTube.Default;
            // gets a Video object with info about the video
            YouTubeVideo video = youTube.GetVideo(url);

            try
            {
                await File.WriteAllBytesAsync(VideoFile, video.GetBytes());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DownloadVideo(int number)
        {
            DownloadUrl(SearchList[number -1].Url);
        }

        /// <summary>
        /// Play a downloaded video
        /// </summary>
        private async void PlayAudio()
        {
            // WTF is this?!
            string file = VideoFile;
            // send a speaking indicator
            await Commands.Connection.SendSpeakingAsync();
            if (int.TryParse(file, out int number))
            {
                if (number > 0 && number < SearchList.Count)
                {
                    DownloadVideo(number);
                    file = VideoFile;
                }
            }

            // Start ffmpeg
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $@"-i ""{file}"" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            ffmpeg = Process.Start(psi);
            if (ffmpeg == null)
            {
                throw new Exception("Couldn't start ffmpeg with file: " + file);
            }
            Stream ffout = ffmpeg.StandardOutput.BaseStream;

            // Play the audio
            byte[] buff = new byte[3840];
            try
            {
                int br;
                while ((br = ffout.Read(buff, 0, buff.Length)) > 0)
                {
                    // not a full sample, mute the rest
                    if (br < buff.Length)
                    {
                        for (int i = br; i < buff.Length; i++)
                        {
                            buff[i] = 0;
                        }
                    }
                    for (int i = 0; i < buff.Length / 2; ++i)
                    {

                        // convert to 16-bit
                        // scale
                        short sample = (short)((buff[i * 2 + 1] << 8) | buff[i * 2]);
                        // value between 0 and 1.0
                        const double gain = 0.2;
                        sample = (short)(sample * gain);

                        // back to byte[]
                        buff[i * 2 + 1] = (byte)(sample >> 8);
                        buff[i * 2] = (byte)(sample & 0xff);
                    }


                    await Commands.Connection.SendAsync(buff, 20);
                }
                while ((br = ffout.Read(buff, 0, buff.Length)) > 0)
                {
                    // not a full sample, mute the rest
                    if (br < buff.Length)
                    {
                        for (int i = br; i < buff.Length; i++)
                        {
                            buff[i] = 0;
                        }
                    }
                    await Commands.Connection.SendAsync(buff, 20);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                // we're not speaking anymore
                await Commands.Connection.SendSpeakingAsync(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
