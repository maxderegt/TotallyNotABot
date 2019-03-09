using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TotallyNotABot.audio;
using TotallyNotABot.DiscordFormat;

namespace TotallyNotABot.PlayList
{
    class Playlist
    {
        public List<PlaylistSong> Songs { get; private set; }
        public string Name { get; }
        public int Index { get; set; }

        public Playlist()
        {
            Index = 0;
            Songs = new List<PlaylistSong>();
        }

        public Playlist(string name)
        {
            this.Name = name;
            Index = 0;
            Songs = new List<PlaylistSong>();
        }

        public void Add(Song song, bool keep = false)
        {
            Songs.Add(new PlaylistSong(song, keep));
        }

        public void Add(PlaylistSong song)
        {
            Songs.Add(song);
        }

        public void Restart()
        {
            Index = 0;
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
            Index = 0;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(DiscordString.Bold("Current playlist\n").Underline().ToString());
            for (int i = 0; i < Songs.Count; i++)
            {
                PlaylistSong song = Songs[i];
                builder.Append($"{DiscordString.Bold($"{i + 1}:")} {song.Song.Title}");
                if (i == Index - 1)
                {
                    builder.Append($" - {DiscordString.Bold("Currently playing!")}");
                }

                builder.Append("\n");
            }
            return builder.ToString();
        }

        public List<string> ToStringList()
        {
            List<string> list = new List<string>();

            int length = 0;
            StringBuilder currentList = new StringBuilder();
            for (int i = 0; i < Songs.Count; i++)
            {

                StringBuilder builder = new StringBuilder();
                if (i == 0)
                {
                    builder.Append(DiscordString.Bold("Current playlist\n").Underline().ToString());
                }

                PlaylistSong song = Songs[i];
                builder.Append($"{DiscordString.Bold($"{i + 1}:")} {song.Song.Title}");
                if (i == Index - 1)
                {
                    builder.Append($" - {DiscordString.Bold("Currently playing!")}");
                }

                builder.Append("\n");
                length += builder.Length;
                if (length > 2000)
                {
                    list.Add(currentList.ToString());
                    currentList.Clear();
                    length = builder.Length;
                }
                currentList.Append(builder.ToString());
            }
            list.Add(currentList.ToString());
            return list;
        }
    }
}
