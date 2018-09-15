using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TotallyNotABot.audio;

namespace TotallyNotABot.PlayList
{
    class Storage
    {
        public static string BASE_DIR = "discordbot\\playlists";
        public static List<Playlist> PlayLists = new List<Playlist>();

        /// <summary>
        /// Create a json object from a playlist
        /// </summary>
        /// <param name="playList"></param>
        /// <returns>The created json object</returns>
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

        /// <summary>
        /// Create a json object from a PlaylistSong
        /// </summary>
        /// <param name="song"></param>
        /// <returns>The created json object</returns>
        public static JObject SongToJson(PlaylistSong song)
        {
            return new JObject {{"title", song.Song.Title}, {"keep", song.Keep}, {"id", song.Song.Id}};
        }

        /// <summary>
        /// Create a json object from a song
        /// </summary>
        /// <param name="song"></param>
        /// <returns>The created json object</returns>
        public static JObject SongToJson(Song song)
        {
            return new JObject {{"title", song.Title}, {"id", song.Id}};
        }
        
        /// <summary>
        /// Create a playlist from a json object
        /// </summary>
        /// <param name="root"></param>
        /// <returns>The created playlist</returns>
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

        /// <summary>
        /// Search the playlist directory for playLists and load them
        /// </summary>
        /// <returns>List of every playlist in the playLists directory</returns>
        public static List<Playlist> LoadAllPlayLists()
        {
            List<Playlist> playLists = new List<Playlist>();

            if (!Directory.Exists(BASE_DIR)) {
                return playLists;
            }

            DirectoryInfo d = new DirectoryInfo(BASE_DIR);
            FileInfo[] files = d.GetFiles("*.json");
            foreach (FileInfo file in files)
            {
                string name = file.Name.Substring(0, file.Name.Length - 5);
                playLists.Add(LoadPlaylist(name));
            }

            PlayLists = playLists;
            return playLists;
        }

        /// <summary>
        /// Create a song from a json object
        /// </summary>
        /// <param name="jSong"></param>
        /// <returns>The created song</returns>
        public static PlaylistSong SongFromJson(JObject jSong)
        {
            Song song = new Song((string)jSong["title"], (string)jSong["id"]);
            return new PlaylistSong(song, (bool)jSong["keep"]);
        }

        /// <summary>
        /// Save a playlist
        /// </summary>
        /// <param name="playlist"></param>
        public static void SavePlayList(Playlist playlist)
        {
            if (!Directory.Exists(BASE_DIR))
            {
                Directory.CreateDirectory(BASE_DIR);
            }
            string path = $"{BASE_DIR}\\{playlist.Name}.json";
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

        /// <summary>
        /// Load a playlist based on its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The returned playlist may be null if it doesn't exist</returns>
        public static Playlist LoadPlaylist(string name)
        {
            string path = $"{BASE_DIR}\\{name}.json";
            if (!File.Exists(path)) return null;
            string rawJson = File.ReadAllText(path);
            JObject root = JObject.Parse(rawJson);
            return PlaylistFromJson(root);
        }
    }
}
