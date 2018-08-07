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
        public List<YoutubeExplode.Models.Video> List = new List<YoutubeExplode.Models.Video>();
        public List<YoutubeExplode.Models.Video> PlayList = new List<YoutubeExplode.Models.Video>();
        public Queue<YoutubeExplode.Models.Video> QueueList = new Queue<YoutubeExplode.Models.Video>();

        public Process ffmpeg { get; set; }
        public static string VideoFile = "discordbot\\video.webm";

        public Audio()
        {
        }

        public void CheckQueue()
        {
            if (Commands.Connection == null) return;
            if (QueueList.Count > 0)
            {
                Video video = QueueList.Dequeue();
                DownloadUrl("https://www.youtube.com/watch?v=" + video.Id);
                DiscordGame test = new DiscordGame
                {
                    Name = video.Title,
                    Details = "",
                    State = "playing music"
                };
                Commands.Discord.UpdateStatusAsync(game: test);
                PlayAudio(VideoFile);
            }
            else
            {
                Commands.Discord.UpdateStatusAsync(null);
            }
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

        public void PlayFromList(int number)
        {
            string url = "https://www.youtube.com/watch?v=" + List[number - 1].Id;
            DownloadUrl(url);
        }

        public async void PlayAudio(string file)
        {
            // send a speaking indicator
            await Commands.Connection.SendSpeakingAsync();
            if (int.TryParse(file, out int number))
            {
                if (number > 0 && number < List.Count)
                {
                    PlayFromList(number);
                    file = VideoFile;
                }
            }

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
                CheckQueue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
