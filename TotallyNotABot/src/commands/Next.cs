using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;
using TotallyNotABot.DiscordFormat;

namespace TotallyNotABot.src.commands
{
    class Next : BaseCommand
    {
        public Next(string name) : base(name)
        {
        }

        public async Task RunCommand(CommandContext ctx, Player player)
        {
            player.Next();
        }

        public override string Help()
        {
            return (DiscordString.Bold("Next: ") + "\nUse !next to skip the current song");
        }
    }
}
