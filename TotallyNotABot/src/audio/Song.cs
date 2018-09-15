using YoutubeExplode.Models;

namespace TotallyNotABot.audio
{
    class Song
    {
        private const string YoutubeUrl = "https://www.youtube.com/watch?v=";
        public string Title { get; }
        public string Id { get;  }
        public string Url => YoutubeUrl + Id;

        public Song(Video youtubeVideo)
        {
            this.Title = youtubeVideo.Title;
            this.Id = youtubeVideo.Id;
        }

        public Song(string title, string id)
        {
            this.Title = title;
            this.Id = id;
        }
    }
}
