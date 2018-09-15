using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VideoLibrary;
using Video = YoutubeExplode.Models.Video;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using CliWrap;
using System.Text.RegularExpressions;

namespace TotallyNotABot.audio
{
    class Source
    {
        public static readonly string VideoFile = "discordbot\\video.webm";
        private static readonly Cli FfmpegCli = new Cli("ffmpeg.exe");

        // List tracking songs resulting from the search command
        public List<Song> SearchList;

        public Source()
        {
            SearchList = new List<Song>();
        }

        public List<Song> Searched(IReadOnlyList<Video> videos)
        {
            this.SearchList.Clear();
            for (int i = 0; i < 5; i++)
            {
                this.SearchList.Add(new Song(videos[i]));
            }
            return SearchList;
        }

        public async Task<string> DownloadSong(Song song)
        {



            //string url = song.Url;
            //// starting point for YouTube actions
            //YouTube youTube = YouTube.Default;
            //// gets a Video object with info about the video
            //YouTubeVideo video = youTube.GetVideo(url);

            //try
            //{
            //    await File.WriteAllBytesAsync(VideoFile, video.GetBytes());
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return await DownloadAndConvertVideoAsync(song); ;
        }

        private static MediaStreamInfo GetBestAudioStreamInfo(MediaStreamInfoSet set)
        {
            if (set.Audio.Count != 0)
                return set.Audio.WithHighestBitrate();
            if (set.Muxed.Count != 0)
                return set.Muxed.WithHighestVideoQuality();
            throw new Exception("No applicable media streams found for this video");
        }

        private static async Task<string> DownloadAndConvertVideoAsync(Song song)
        {
            string id = song.Id;

            Console.WriteLine($"Working on video [{id}]...");


            try
            {

                YoutubeClient youtubeclient = new YoutubeClient();

                // Get video info
                var video = await youtubeclient.GetVideoAsync(id);
                var set = await youtubeclient.GetVideoMediaStreamInfosAsync(id);
                string cleantitle;
                try
                {
                    cleantitle = Regex.Replace(video.Title, @"[^\w\.@-]", "",
                                         RegexOptions.None, TimeSpan.FromSeconds(1.5));
                }
                // If we timeout when replacing invalid characters, 
                // we should return Empty.
                catch (RegexMatchTimeoutException)
                {
                    cleantitle = String.Empty;
                }
                Console.WriteLine($"{video.Title}");

                // Get highest bitrate audio-only or highest quality mixed stream
                var streamInfo = GetBestAudioStreamInfo(set);

                // Download
                Console.WriteLine("Downloading...");
                var streamFileExt = streamInfo.Container.GetFileExtension();
                var streamFilePath = $"discordbot\\video.{streamFileExt}";
                File.Delete(streamFilePath);
                await youtubeclient.DownloadMediaStreamAsync(streamInfo, streamFilePath);

                // Convert to mp3
                //Console.WriteLine("Converting...");
                //var outputFilePath = $"{cleantitle}.mp3";
                //File.Delete(outputFilePath);
                //await FfmpegCli.SetArguments($"-i \"{streamFilePath}\" -q:a 0 -map a \"{outputFilePath}\" -y").ExecuteAsync();


                Console.WriteLine($"Downloaded video [{id}] to [{streamFilePath}]");

                return streamFilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}