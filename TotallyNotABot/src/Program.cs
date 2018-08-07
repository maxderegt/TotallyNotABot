using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.VoiceNext;
using DSharpPlus.CommandsNext;
using TotallyNotABot.src.commands;

namespace TotallyNotABot
{
    class Program
    {
        static DiscordClient discord;
        static VoiceNextClient voice;
        static CommandsNextModule commands;
        static Commands myCommands = new Commands();

        private static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NDIwMzMzNjcyNDU4MzU0Njk4.DX9J-A.npSAUCUOYs-vOSJNwHNQTE3gKKw",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            voice = discord.UseVoiceNext();

            myCommands.setdiscord(discord, voice);

            discord.MessageCreated += async e =>
            {
                string message = e.Message.Content.ToLower();

                if (e.Message.Author.Id != 420333672458354698)
                {
                    Console.WriteLine("recieved message : {0}", e.Message);
                }
            };

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "!",
                EnableDms = false
            });

            commands.RegisterCommands<Commands>();
            
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            Console.WriteLine(e);
            throw new NotImplementedException();
        }
    }
}
