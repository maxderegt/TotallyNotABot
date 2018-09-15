using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

namespace TotallyNotABot.commands
{
    class Leave
    {
        public async Task RunCommand(CommandContext ctx, VoiceNextConnection connection)
        {
            DiscordChannel channel = ctx.Member.VoiceState.Channel;
            if (channel == null)
                Console.WriteLine("Not connected in this guild.");

            await ctx.RespondAsync("Bye Bye");
            connection.Disconnect();
        }
    }
}
