using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VideoLibrary;
using Video = YoutubeExplode.Models.Video;

namespace TotallyNotABot.audio
{
    class Source
    {
        public static readonly string VideoFile = "discordbot\\video.webm";

        // List tracking songs resulting from the search command
        public List<Song> SearchList;

        public Source()
        {
            SearchList = new List<Song>();
        }

        public void Searched(IReadOnlyList<Video> videos)
        {
            this.SearchList.Clear();
            foreach (Video video in videos)
            {
                this.SearchList.Add(new Song(video));
            }
        }

        public async Task<string> DownloadSong(Song song)
        {
            string url = song.Url;
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
            return VideoFile;
        }

    }
}
