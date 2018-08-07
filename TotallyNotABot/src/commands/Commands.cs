using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.VoiceNext;
using TotallyNotABot.audio;

namespace TotallyNotABot.commands
{
    class Commands
    {
        // Discord connection
        public static VoiceNextClient Voice;
        public static VoiceNextConnection Connection;
        public static DiscordClient Discord;

        // Commands
        private static Search _searchCommand;
        private static Start _startCommand;
        private static Play _playCommand;
        private static Stop _stopCommand;
        private static Join _joinCommand;
        private static Spam _spamCommand;
        private static Leave _leaveCommand;

        // Other stuff
        private static Audio _audio;

        public static void Init(DiscordClient client, VoiceNextClient voice)
        {
            Discord = client;
            Voice = voice;
            _audio = new Audio();

            // Commands
            _searchCommand = new Search();
            _startCommand = new Start();
            _playCommand = new Play();
            _stopCommand = new Stop();
            _joinCommand = new Join();
            _spamCommand = new Spam();
            _leaveCommand = new Leave();
        }


        [Command("search")]
        public async Task Search(CommandContext ctx)
        {
            await _searchCommand.RunCommand(ctx, _audio);
        }

        [Command("spam")]
        public async Task Spam(CommandContext ctx)
        {
            await _spamCommand.RunCommand(ctx, Discord);
        }

        [Command("start")]
        public async Task Start(CommandContext ctx)
        {
            await _startCommand.RunCommand(ctx, _audio);
        }

        [Command("play")]
        public async Task Play(CommandContext ctx)
        {
            await _playCommand.RunCommand(ctx, _audio);
        }

        [Command("stop")]
        public async Task Stop(CommandContext ctx)
        {
            await _stopCommand.RunCommand(ctx, _audio);
        }

        [Command("join")]
        public async Task Join(CommandContext ctx)
        {
            Connection = await _joinCommand.RunCommand(ctx, _audio, Connection, Voice);
        }

        [Command("leave")]
        public async Task Leave(CommandContext ctx)
        {
            await _stopCommand.RunCommand(ctx, _audio);
            await _leaveCommand.RunCommand(ctx, Connection);
        }
    }
}
