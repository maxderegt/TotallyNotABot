using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;
using TotallyNotABot.DiscordFormat;

namespace TotallyNotABot.commands
{
    class Stop : src.commands.BaseCommand
    {
        public Stop(string name) : base(name)
        {
        }

        public async Task RunCommand(CommandContext ctx, Player player)
        {
            player.Stop();
            await ctx.RespondAsync($"👌");
        }

        public override string Help()
        {
            return (DiscordString.Bold("Stop: ") + "\nUse !stop to stop bot playing music");
        }
    }
}
