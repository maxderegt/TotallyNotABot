using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using TotallyNotABot.audio;

namespace TotallyNotABot.commands
{
    class Join
    {
        public async Task<VoiceNextConnection> RunCommand(CommandContext ctx, Audio audio, VoiceNextConnection connection, VoiceNextClient voice)
        {
            if (connection != null) return connection;
            try
            {
                audio.ffmpeg.Kill();
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
    }
}
