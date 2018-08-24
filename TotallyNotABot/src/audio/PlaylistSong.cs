using System;
using System.Xml.Serialization;

namespace TotallyNotABot.audio
{
    [Serializable]
    class PlaylistSong
    {
        [XmlAttribute]
        public Song Song { get; }
        public bool Keep { get; }

        public PlaylistSong(Song song, bool keep)
        {
            this.Song = song;
            this.Keep = keep;
        }
    }
}
