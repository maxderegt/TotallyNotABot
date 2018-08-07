using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;
using TotallyNotABot.src.audio;

namespace TotallyNotABot.src.commands
{
    class Join
    {
        public async Task<VoiceNextConnection> RunCommand(CommandContext ctx, Audio audio, VoiceNextConnection connection, VoiceNextClient voice)
        {
            if (connection == null)
            {
                try
                {
                    audio.ffmpeg.Kill();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                var channel = ctx.Member.VoiceState.Channel;
                if (channel == null)
                    Console.WriteLine("You need to be in a voice channel.");
                else
                {
                    connection = await voice.ConnectAsync(channel);
                    Console.WriteLine("connection established");
                }
            }

            return connection;
        }
    }
}
