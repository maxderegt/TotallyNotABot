using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;
using YoutubeExplode;

namespace TotallyNotABot.commands
{
    class Search
    {
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

            // Get the youtube videos
            YoutubeClient client = new YoutubeClient();
            List<Song> list = player.Searched(await client.SearchVideosAsync(input, 1));
            StringBuilder builder = new StringBuilder("Current playlist");
            for (int i = 0; i < list.Count; i++)
            {
                Song song = list[i];
                builder.Append($"@\n{i+1}: {song.Title}");
            }
            await ctx.RespondAsync(builder.ToString());
            // Play the song if the play flag was set
            if (play)
            {
                player.Play();
            }
        }
    }
}
