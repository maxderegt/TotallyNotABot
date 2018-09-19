using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;
using YoutubeExplode;
using TotallyNotABot.DiscordFormat;

namespace TotallyNotABot.src.commands
{
    class Search : BaseCommand
    {
        public Search(string name) : base(name)
        {
        }

        public static async Task<List<Song>> DoSearch(string title, Player player)
        {
            // Get the youtube videos
            YoutubeClient client = new YoutubeClient();
            List<Song> list = player.Searched(await client.SearchVideosAsync(title, 1));
            return list;
        }

        public async Task RunCommand(CommandContext ctx, Player player)
        {
            // Parse user input    
            string[] msg = ctx.Message.Content.Split(" ");
            string input = "";
            bool play = false;
            if (msg[1].Equals("-p") && msg.Length != 2)
            {
                play = true;
                input = input + msg[2];
                for (int i = 3; i < msg.Length; i++)
                {
                    input = input + " " + msg[i];
                }
            }
            else
            {
                input = input + msg[1];
                for (int i = 2; i < msg.Length; i++)
                {
                    input = input + " " + msg[i];
                }
            }

            List<Song> list = await DoSearch(input, player);
            StringBuilder builder = new StringBuilder(DiscordString.Bold("Search results:").Underline().ToString());
            for (int i = 0; i < list.Count; i++)
            {
                Song song = list[i];
                builder.Append($"\n{DiscordString.Bold($"{i+1}:")} {song.Title}");
            }
            await ctx.RespondAsync(builder.ToString());
            // Play the song if the play flag was set
            if (play)
            {
                player.Play();
            }
        }

        public override string Help()
        {
            return (DiscordString.Bold("Search: ") + "\nUse !search [name of song] to search for a song to play/add to playlist");
        }
    }
}
