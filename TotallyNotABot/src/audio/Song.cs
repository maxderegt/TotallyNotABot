using System;
using System.Xml.Serialization;
using YoutubeExplode.Models;

namespace TotallyNotABot.audio
{
    [Serializable]
    class Song
    {
        private const string YoutubeUrl = "https://www.youtube.com/watch?v=";
        [XmlAttribute]
        public string Title { get; }
        [XmlAttribute]
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
