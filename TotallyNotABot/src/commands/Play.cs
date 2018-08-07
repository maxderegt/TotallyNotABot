using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.src.audio;

namespace TotallyNotABot.src.commands
{
    class Play
    {
        public async Task RunCommand(CommandContext ctx, Audio audio)
        {
            string[] msg = ctx.Message.Content.Split(" ");
            if (msg.Length > 1)
            {
                if (audio.list.Count > 0)
                {
                    if (int.TryParse(msg[1], out int number))
                    {
                        string url = "https://www.youtube.com/watch?v=" + audio.list[number - 1].Id;
                        audio.QueueList.Enqueue(audio.list[number - 1]);
                    }
                    await ctx.RespondAsync($"Added to queue");

                    List<string> templist = new List<string>();

                    foreach (YoutubeExplode.Models.Video item in audio.QueueList)
                    {
                        templist.Add(item.Title);
                    }
                    await ctx.RespondAsync($"{String.Join("\n", templist)}");

                    if (audio.ffmpeg == null)
                        audio.CheckQueue();
                    else if (audio.ffmpeg.HasExited)
                        audio.CheckQueue();
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
