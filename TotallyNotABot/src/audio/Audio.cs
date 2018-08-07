using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using VideoLibrary;

namespace TotallyNotABot.src.audio
{
    class Audio
    {
        public List<YoutubeExplode.Models.Video> list = new List<YoutubeExplode.Models.Video>();
        public List<YoutubeExplode.Models.Video> PlayList = new List<YoutubeExplode.Models.Video>();
        public Queue<YoutubeExplode.Models.Video> QueueList = new Queue<YoutubeExplode.Models.Video>();

        public Process ffmpeg { get; set; }
        static DiscordClient discord;
        public static string videoFile = "discordbot\\video.webm";
        private VoiceNextConnection connection;

        public Audio(VoiceNextConnection connection)
        {
            this.connection = connection;
        }

        public void CheckQueue()
        {
            if (connection != null)
                if (QueueList.Count > 0)
                {
                    var video = QueueList.Dequeue();
                    DownloadURL("https://www.youtube.com/watch?v=" + video.Id);
                    DiscordGame test = new DiscordGame
                    {
                        Name = video.Title,
                        Details = "",
                        State = "playing music"
                    };
                    discord.UpdateStatusAsync(game: test);
                    PlayAudio(videoFile);
                }
                else
                {
                    discord.UpdateStatusAsync(null);
                }
        }

        public async void DownloadURL(string url)
        {
            var youTube = YouTube.Default; // starting point for YouTube actions
            var video = youTube.GetVideo(url); // gets a Video object with info about the video

            try
            {
                await File.WriteAllBytesAsync(videoFile, video.GetBytes());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void PlayFromList(int number)
        {
            string url = "https://www.youtube.com/watch?v=" + list[number - 1].Id;
            DownloadURL(url);
        }

        public async void PlayAudio(string file)
        {
            await connection.SendSpeakingAsync(true); // send a speaking indicator
            int number;
            if (int.TryParse(file, out number))
            {
                if (number > 0 && number < list.Count)
                {
                    PlayFromList(number);
                    file = videoFile;
                }
            }

            var psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $@"-i ""{file}"" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            ffmpeg = Process.Start(psi);
            var ffout = ffmpeg.StandardOutput.BaseStream;

            var buff = new byte[3840];
            var br = 0;

            try
            {
                while ((br = ffout.Read(buff, 0, buff.Length)) > 0)
                {
                    if (br < buff.Length) // not a full sample, mute the rest
                        for (var i = br; i < buff.Length; i++)
                            buff[i] = 0;

                    for (int i = 0; i < buff.Length / 2; ++i)
                    {

                        // convert to 16-bit
                        short sample = (short)((buff[i * 2 + 1] << 8) | buff[i * 2]);// scale
                        const double gain = 0.2; // value between 0 and 1.0
                        sample = (short)(sample * gain);

                        // back to byte[]
                        buff[i * 2 + 1] = (byte)(sample >> 8);
                        buff[i * 2] = (byte)(sample & 0xff);
                    }


                    await connection.SendAsync(buff, 20);
                }
                while ((br = ffout.Read(buff, 0, buff.Length)) > 0)
                {
                    if (br < buff.Length) // not a full sample, mute the rest
                        for (var i = br; i < buff.Length; i++)
                            buff[i] = 0;

                    await connection.SendAsync(buff, 20);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                await connection.SendSpeakingAsync(false); // we're not speaking anymore
                CheckQueue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
