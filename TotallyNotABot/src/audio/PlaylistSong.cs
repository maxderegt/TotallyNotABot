namespace TotallyNotABot.audio
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
