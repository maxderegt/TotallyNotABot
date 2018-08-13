using System.Collections.Generic;

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

            // Return the song if it can be kept
            PlaylistSong target = Songs[Index];
            if (target.Keep)
            {
                // Increase the index
                Index++;
                return target.Song;
            }

            // Remove the song from the list and return it if it can't be kept
            // Don't increase the index, because a song was removed.
            List<PlaylistSong> temp = new List<PlaylistSong>(Songs.Count - 1);
            for (int i = 0; i < Songs.Count; i++)
            {
                if (i != Index)
                {
                    temp.Add(Songs[i]);
                }
            }
            Songs = temp;
            return target.Song;
        }

        public override string ToString()
        {
            // TODO: implement!
            return "";
        }
    }
}
