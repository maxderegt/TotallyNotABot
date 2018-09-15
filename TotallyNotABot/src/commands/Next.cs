using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;

namespace TotallyNotABot.src.commands
{
    class Next
    {
        public async Task RunCommand(CommandContext ctx, Player player)
        {
            player.Next();
        }
    }
}
