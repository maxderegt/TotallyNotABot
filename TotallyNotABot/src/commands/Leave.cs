using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;

namespace TotallyNotABot.src.commands
{
    class Leave
    {
        public async Task RunCommand(CommandContext ctx, VoiceNextConnection connection)
        {
            var channel = ctx.Member.VoiceState.Channel;
            if (channel == null)
                Console.WriteLine("Not connected in this guild.");

            await ctx.RespondAsync("Bye Bye");
            connection.Disconnect();
            connection = null;
        }
    }
}
