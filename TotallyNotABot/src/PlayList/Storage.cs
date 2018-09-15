using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TotallyNotABot.audio;

namespace TotallyNotABot.PlayList
{
    class Storage
    {
        /**
         * Create json
         */
        public static JObject PlaylistToJson(Playlist playList)
        {
            JObject root = new JObject {{"name", playList.Name}};
            JArray songArray = new JArray();
            foreach (PlaylistSong song in playList.Songs)
            {
                songArray.Add(SongToJson(song));
            }

            root.Add("songs", songArray);

            return root;
        }

        public static JObject SongToJson(PlaylistSong song)
        {
            return new JObject {{"title", song.Song.Title}, {"keep", song.Keep}, {"id", song.Song.Id}};
        }

        public static JObject SongToJson(Song song)
        {
            return new JObject {{"title", song.Title}, {"id", song.Id}};
        }

        /**
         * From Json
         */
        public static Playlist PlaylistFromJson(JObject root)
        {
            Playlist playlist = new Playlist((string)root["name"]);
            JArray jsonSongs = (JArray)root["songs"];
            foreach (JToken jObj in jsonSongs)
            {
                playlist.Add(SongFromJson((JObject)jObj));
            }

            return playlist;
        }

        public static PlaylistSong SongFromJson(JObject jSong)
        {
            Song song = new Song((string)jSong["title"], (string)jSong["id"]);
            return new PlaylistSong(song, (bool)jSong["keep"]);
        }

        /**
         * Save/Load file
         */
        public static void SavePlayList(Playlist playlist)
        {
            string path = $"discordbot\\{playlist.Name}.json";
            if (!File.Exists(path))
            {
                File.Delete(path);
            }

            JObject jsonPlayList = PlaylistToJson(playlist);
            StreamWriter writer = new StreamWriter(path);
            JsonWriter jWriter = new JsonTextWriter(writer);
            jsonPlayList.WriteTo(jWriter);
            jWriter.Close();
        }

        public static Playlist LoadPlaylist(string name)
        {
            string path = $"discordbot\\{name}.json";
            if (!File.Exists(path)) return null;
            string rawJson = File.ReadAllText(path);
            JObject root = JObject.Parse(rawJson);
            return PlaylistFromJson(root);
        }
    }
}
