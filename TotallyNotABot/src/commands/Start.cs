using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.src.audio;

namespace TotallyNotABot.src.commands
{
    class Start
    {
        public async Task RunCommand(CommandContext ctx, Audio audio)
        {
            if (audio.ffmpeg == null)
            {
                audio.CheckQueue();
            }
            else if (audio.ffmpeg.HasExited)
            {
                audio.CheckQueue();
            }
            else
            {
                await ctx.RespondAsync("Music is already being played");
            }
        }
    }
}
