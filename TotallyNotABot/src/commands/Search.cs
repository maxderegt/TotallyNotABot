using System;
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
            player.Searched(await client.SearchVideosAsync(input, 1));

            // Play the song if the play flag was set
            if (play)
            {
                player.Play();
            }
        }
    }
}
