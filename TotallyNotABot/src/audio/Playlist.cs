using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using TotallyNotABot.commands;

namespace TotallyNotABot.audio
{
    class Playlist
    {
        [XmlAttribute]
        public List<PlaylistSong> Songs { get; private set; }

        public int Index { get; set; }

        public Playlist()
        {
            Index = 0;
            Songs = new List<PlaylistSong>();
        }

        public void Add(Song song, bool keep = false)
        {
            Songs.Add(new PlaylistSong(song, keep));
        }

        public Song Next()
        {
            if (Index < 0 || Index >= Songs.Count)
            {
                return null;
            }

            if (Index > 0)
            {
                PlaylistSong previous = Songs[Index - 1];
                if (!previous.Keep)
                {
                    Songs.Remove(previous);
                    Index--;
                }
            }

            PlaylistSong target = Songs[Index];
                // Increase the index
            Index++;
            return target.Song;
        }

        /// <summary>
        /// Remove any songs where keep == false
        /// </summary>
        public void Clear()
        {
            Songs = Songs.Where(song => song.Keep).ToList();
        }

        public void ToXml()
        {
            XmlDocument doc = new XmlDocument();
            foreach (PlaylistSong song in Songs)
            {
                XmlElement songElement = (XmlElement) doc.AppendChild(doc.CreateElement("song"));
                XmlElement idElement = (XmlElement) doc.CreateElement("id");
                idElement.InnerText = song.Song.Id;
                songElement.AppendChild(idElement);
                XmlElement titleElemt = doc.CreateElement("title");
                titleElemt.InnerText = song.Song.Title;
                songElement.AppendChild(titleElemt);
            }

            if (File.Exists("playList.xml"))
            {
                File.Delete("playList.xml");
            }
            File.WriteAllText("playList.xml", doc.OuterXml);
        }

        public override string ToString()
        {
            ToXml();
            StringBuilder builder = new StringBuilder("Current playlist \n");
            for (int i = 0; i < Songs.Count; i++)
            {
                PlaylistSong song = Songs[i];
                builder.Append($"{i + 1}: {song.Song.Title}");
                if (i == Index - 1)
                {
                    builder.Append(" - Currently playing!");
                }

                builder.Append("\n");
            }
            return builder.ToString();
        }
    }
}
