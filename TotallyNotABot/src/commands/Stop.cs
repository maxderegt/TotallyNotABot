using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.src.audio;

namespace TotallyNotABot.src.commands
{
    class Stop
    {
        public async Task RunCommand(CommandContext ctx, Audio audio)
        {
            try
            {
                audio.ffmpeg.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            await ctx.RespondAsync($"👌");
        }
    }
}
