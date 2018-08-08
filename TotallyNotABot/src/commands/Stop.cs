using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;

namespace TotallyNotABot.commands
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
