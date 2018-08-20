using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;

namespace TotallyNotABot.commands
{
    class Stop
    {
        public async Task RunCommand(CommandContext ctx, Player player)
        {
            player.Stop();
            await ctx.RespondAsync($"👌");
        }
    }
}
