using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;

namespace TotallyNotABot.commands
{
    class Start
    {
        public async Task RunCommand(CommandContext ctx, Audio audio)
        {
            if (audio.ffmpeg == null || audio.ffmpeg.HasExited)
            {
                audio.Start();
            }
            else
            {
                await ctx.RespondAsync("Music is already being played");
            }
        }
    }
}
