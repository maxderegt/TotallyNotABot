using TotallyNotABot.audio;

namespace TotallyNotABot.PlayList
{
    class PlaylistSong
    {
        public Song Song { get; }
        public bool Keep { get; }

        public PlaylistSong(Song song, bool keep)
        {
            this.Song = song;
            this.Keep = keep;
        }
    }
}
