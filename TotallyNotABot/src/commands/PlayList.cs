using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;
using TotallyNotABot.PlayList;
using TotallyNotABot.DiscordFormat;

namespace TotallyNotABot.commands
{
    class PlayList : src.commands.BaseCommand
    {
        public readonly List<string> commands = new List<string>(new string[] { "create", "add", "delete", "play", "show", "shuffle" });
        private CommandContext ctx;
        private Player player;
        private string[] msg;

        public PlayList(string name) : base(name)
        {
        }

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
                    await Delete();
                    break;
                case "play":
                    await Play();
                    break;
                case "shuffle":
                    Shuffle();
                    break;
                case "show":
                    await Show();
                    break;
            }
            return -1;
        }

        public async void Shuffle()
        {
            int i = await CheckForPlaylist();
            if (i != -1)
            {
                Playlist playlist = Storage.PlayLists[i];
                playlist.Shuffle();
            }

            await ctx.RespondAsync("Playlist shuffled");
        }

        private async Task<int> CheckForPlaylist()
        {
            int i = 0;
            foreach (Playlist playlist in Storage.PlayLists)
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
                Storage.PlayLists.Add(new Playlist(msg[2]));
                await ctx.RespondAsync("Created playlist: " + msg[2]);
            }
        }
        private async Task Add()
        {
            int i = await CheckForPlaylist();
            if (i != -1) { 
                Playlist list = Storage.PlayLists[i];
                try
                {
                    int i2 = 0;
                    int.TryParse(msg[3], out i2);
                    if (player.HasSearch())
                    {
                        list.Add(player.source.SearchList[i2 - 1], true);
                        Storage.SavePlayList(Storage.PlayLists[i]);
                        await ctx.RespondAsync("Added song " + DiscordString.Bold(player.source.SearchList[i2 - 1].Title) + " to playlist " + DiscordString.Bold(list.Name) + "\n"+ list.ToString());
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    await ctx.RespondAsync("Could not add song to playlist due to unknow error");
                }
            }
        }
        private async Task Delete()
        {
            int i = await CheckForPlaylist();
            if (i != -1)
            {
                Playlist list = Storage.PlayLists[i];
                try
                {
                    try
                    {
                        int i2 = 0;
                        int.TryParse(msg[3], out i2);
                        string name = list.Songs[i2 - 1].Song.Title;
                        list.Songs.RemoveAt(i2 - 1);
                        Storage.SavePlayList(Storage.PlayLists[i]);
                        await ctx.RespondAsync("Deleted song " + DiscordString.Bold(name) + " from playlist " + DiscordString.Bold(list.Name));
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
                await Commands.join(ctx);
                Playlist list = Storage.PlayLists[i];
                list.Index = 0;
                await player.Stop();
                player.Current = list;
                player.Play();
                await ctx.RespondAsync("Playing playlist: " + list.Name);
            }
        }
        private async Task Show()
        {
            int i = await CheckForPlaylist();
            if (i != -1)
            {
                Playlist list = Storage.PlayLists[i];
                await ctx.RespondAsync(list.ToString());
            }
        }

        public override string Help()
        {
            return (DiscordString.Bold("Playlist: ") +
                "\nUse !playlist create [name] to create a new playlist" +
                "\nUse !playlist add [name of playlist] [number] to add a song from !search to a playlist" +
                "\nUse !playlist delete [name of playlist] [number] to delete a song from the playlist" +
                "\nUse !playlist play [name of playlist] to play a playlist"  +
                "\nUse !playlist show to show the songs in a playlist");
        }
    }
}