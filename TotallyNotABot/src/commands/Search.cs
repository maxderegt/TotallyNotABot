using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.src.audio;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace TotallyNotABot.src.commands
{
    class Search
    {
        public async Task RunCommand(CommandContext ctx, Audio audio)
        {
            audio.list.Clear();

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
            YoutubeClient client = new YoutubeClient();
            IReadOnlyList<Video> temp = await client.SearchVideosAsync(input, 1);
            for (int i = 0; i < 5; i++)
            {
                audio.list.Add(temp[i]);
            }
            await ctx.RespondAsync($"{string.Join("\n", audio.list)}");
            if (play)
            {
                audio.PlayFromList(1);
                audio.PlayAudio(Audio.videoFile);
            }
        }
    }
}
