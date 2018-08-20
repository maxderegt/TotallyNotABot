﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TotallyNotABot.audio
{
    class Playlist
    {
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

        public override string ToString()
        {
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
