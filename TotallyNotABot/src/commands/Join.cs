using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using TotallyNotABot.audio;
using TotallyNotABot.DiscordFormat;

namespace TotallyNotABot.src.commands
{
    class Join : BaseCommand
    {
        public Join(string name) : base(name)
        {
        }

        public async Task<VoiceNextConnection> RunCommand(CommandContext ctx, Player player, VoiceNextConnection connection, VoiceNextClient voice)
        {
            if (connection != null) return connection;
            try
            {
                player.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            DiscordChannel channel = ctx.Member.VoiceState.Channel;
            if (channel == null)
            {
                await ctx.RespondAsync("You need to be in a voice channel.");
            }
            else
            {
                connection = await voice.ConnectAsync(channel);
                Console.WriteLine("connection established");
            }

            return connection;
        }

        public override string Help()
        {
            return (DiscordString.Bold("Join: ") + "\nUse !join when you are in a voice channel to let the bot join your voicechannel");
        } 
    }
}
