using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using TotallyNotABot.DiscordFormat;

namespace TotallyNotABot.commands
{
    class Leave : src.commands.BaseCommand
    {
        public Leave(string name) : base(name)
        {
        }

        public async Task RunCommand(CommandContext ctx, VoiceNextConnection connection)
        {
            DiscordChannel channel = ctx.Member.VoiceState.Channel;
            if (channel == null)
                Console.WriteLine("Not connected in this guild.");

            await ctx.RespondAsync("Bye Bye");
            connection.Disconnect();
        }

        public override string Help()
        {
            return (DiscordString.Bold("Leave: ") + "\nUse !leave to stop the bot and dismiss the bot from the voicechannel");
        }
    }
}
