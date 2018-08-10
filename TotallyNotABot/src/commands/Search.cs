using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace TotallyNotABot.commands
{
    class Search
    {
        public async Task RunCommand(CommandContext ctx, Audio audio)
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
            audio.Searched(await client.SearchVideosAsync(input, 1));

            // Respond to the user
            if (play)
            {
                // Download the video
                audio.DownloadVideo(1);
                try
                {
                    audio.PlayAudio();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                await ctx.RespondAsync($"{string.Join("\n", audio.SearchList)}");

            }
        }
    }
}
