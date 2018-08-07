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
            audio.List.Clear();

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
                audio.List.Add(temp[i]);
            }
            await ctx.RespondAsync($"{string.Join("\n", audio.List)}");
            if (play)
            {
                audio.PlayFromList(1);
                audio.PlayAudio(Audio.VideoFile);
            }
        }
    }
}
