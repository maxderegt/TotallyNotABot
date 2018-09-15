using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;
using TotallyNotABot.PlayList;

namespace TotallyNotABot.commands
{
    class PlayList
    {
        public List<Playlist> playlists = new List<Playlist>();
        public readonly List<string> commands = new List<string>(new string[] { "create", "add", "delete", "play", "show" });
        private CommandContext ctx;
        private Player player;
        private string[] msg;

        public async Task RunCommand(CommandContext ctx, Player player)
        {
            this.ctx = ctx;
            this.player = player;
            msg = ctx.Message.Content.Split(" ");
            await CheckMessage();
        }


        private async Task<int> CheckMessage()
        {
            // Length of the message
            if (msg.Length <= 2)
            {
                await ctx.RespondAsync($"Please use the command !playlist [command] [name] (optional)[1-5]");
                return -1;
            }
            string command = msg[1];
            if (!commands.Contains(command))
            {
                await ctx.RespondAsync("Invalid command");
                return -1;
            }

            switch (command)
            {
                case "create":
                    await Create();
                    break;
                case "add":
                    await Add();
                    break;
                case "delete":
                    break;
                case "play":
                    await Play();
                    break;
                case "show":
                    await Show();
                    break;
            }
            return -1;
        }

        private async Task<int> CheckForPlaylist()
        {
            int i = 0;
            foreach (Playlist playlist in playlists)
            {
                if (playlist.Name == msg[2])
                    return i;
                i++;
            }
            await ctx.RespondAsync("No playlist with that name");
            return -1;
        }

        private async Task Create()
        {
            if(msg.Length > 3)
            {
                await ctx.RespondAsync("Please only 1 word for the name");
            }
            else
            {
                playlists.Add(new Playlist(msg[2]));
            }
        }
        private async Task Add()
        {
            int i = await CheckForPlaylist();
            if (i != -1) { 
                Playlist list = playlists[i];
                try
                {
                    int i2 = 0;
                    int.TryParse(msg[3], out i2);
                    if(player.HasSearch())
                        list.Add(player.source.SearchList[i2-1]);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    await ctx.RespondAsync("could not add song to playlist due to unknow error");
                }
            }
        }
        private async Task Delete()
        {
            int i = await CheckForPlaylist();
            if (i != -1)
            {
                Playlist list = playlists[i];
                try
                {
                    try
                    {
                        int i2 = 0;
                        int.TryParse(msg[3], out i2);
                        list.Songs.RemoveAt(i2 - 1);
                    }
                    catch(Exception ex)
                    {
                        //TODO delete by string
                        Console.WriteLine(ex);
                        //Console.WriteLine("Number didn't work, trying string now");
                        //try
                        //{
                        //    for (int i2 = 3; i2 < msg.Length; i2++) ;
                        //}
                        //catch (Exception ex2)
                        //{
                        //    Console.WriteLine(ex);
                        //    await ctx.RespondAsync("Wrong format use: !playlist delete [playlist] [number/name of song]");
                        //}
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    await ctx.RespondAsync("Wrong format use: !playlist delete [playlist] [number/name of song]");
                }

            }
        }
        private async Task Play()
        {
            int i = await CheckForPlaylist();
            if (i != -1)
            {
                Playlist list = playlists[i];
                await player.Stop();
                player.Current = list;
                player.Play();
            }
        }
        private async Task Show()
        {
            int i = await CheckForPlaylist();
            if (i != -1)
            {
                Playlist list = playlists[i];
                await ctx.RespondAsync(list.ToString());
            }
        }
    }
}