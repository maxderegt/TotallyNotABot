using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;
using TotallyNotABot.audio;
using TotallyNotABot.commands;
using TotallyNotABot.core;
using TotallyNotABot.PlayList;

namespace TotallyNotABot
{
    class Program
    {
        private static void Main(string[] args)
        {
            if (!Settings.Load()) {
                return;
            }

            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            DiscordClient discord = new DiscordClient(new DiscordConfiguration
            {
                Token = Settings.Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            VoiceNextClient voice = discord.UseVoiceNext();

            Commands.Init(discord, voice);

            discord.MessageCreated += async e =>
            {
                if (e.Message.Author.Id != 420333672458354698)
                {
                    Console.WriteLine("recieved message : {0}", e.Message);
                }
            };

            CommandsNextModule commandsModule = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = Settings.Prefix,
                EnableDms = false
            });

            commandsModule.RegisterCommands<Commands>();
            
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
