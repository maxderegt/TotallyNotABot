using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;

namespace TotallyNotABot.commands
{
    class Play
    {
        public async Task RunCommand(CommandContext ctx, Audio audio)
        {
            string[] msg = ctx.Message.Content.Split(" ");
            if (msg.Length > 1)
            {
                if (audio.List.Count > 0)
                {
                    if (int.TryParse(msg[1], out int number))
                    {
                        audio.QueueList.Enqueue(audio.List[number - 1]);
                    }
                    await ctx.RespondAsync($"Added to queue");

                    List<string> templist = new List<string>();

                    foreach (YoutubeExplode.Models.Video item in audio.QueueList)
                    {
                        templist.Add(item.Title);
                    }
                    await ctx.RespondAsync($"{string.Join("\n", templist)}");

                    if (audio.ffmpeg == null)
                    {
                        audio.CheckQueue();
                    }
                    else if (audio.ffmpeg.HasExited)
                    {
                        audio.CheckQueue();
                    }
                }
                else
                {
                    await ctx.RespondAsync($"Please use the command !search [name of a song] first");
                }
            }
            else
            {
                await ctx.RespondAsync($"Please use the command !play [1-5] to select a song from the !search command");
            }
        }
    }
}
