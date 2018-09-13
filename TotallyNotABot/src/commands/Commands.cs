using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.VoiceNext;
using TotallyNotABot.audio;
using TotallyNotABot.src.commands;

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
        private static Next _nextCommand;
        private static PlayList _playListCommand;

        // Other stuff
//        private static Audio _audio;
        private static Player _player;

        public static void Init(DiscordClient client, VoiceNextClient voice)
        {
            Discord = client;
            Voice = voice;
            _player = new Player();

            // Commands
            _searchCommand = new Search();
            _startCommand = new Start();
            _playCommand = new Play();
            _stopCommand = new Stop();
            _joinCommand = new Join();
            _spamCommand = new Spam();
            _leaveCommand = new Leave();
            _nextCommand = new Next();
            _playListCommand = new PlayList();
        }

        [Command("playlist")]
        public async Task Playlist(CommandContext ctx)
        {
            await _playListCommand.RunCommand(ctx, _player);
        }

        [Command("next")]
        public async Task Next(CommandContext ctx)
        {
            await _nextCommand.RunCommand(ctx, _player);
        }

        [Command("search")]
        public async Task Search(CommandContext ctx)
        {
            await _searchCommand.RunCommand(ctx, _player);
        }

        [Command("spam")]
        public async Task Spam(CommandContext ctx)
        {
            await _spamCommand.RunCommand(ctx, Discord);
        }

        [Command("start")]
        public async Task Start(CommandContext ctx)
        {
            _startCommand.RunCommand(ctx, _player);
        }

        [Command("play")]
        public async Task Play(CommandContext ctx)
        {
            await join(ctx);
            if (Connection != null)
            {
                await _playCommand.RunCommand(ctx, _player);
            }
            else
            {
                await ctx.RespondAsync("Can't join channel");
            }
        }

        [Command("stop")]
        public async Task Stop(CommandContext ctx)
        {
            await _stopCommand.RunCommand(ctx, _player);
        }
        
        public async static Task join(CommandContext ctx)
        {
            if (Connection == null)
            {
                Connection = await _joinCommand.RunCommand(ctx, _player, Connection, Voice);
            }
        }

        [Command("join")]
        public async Task Join(CommandContext ctx)
        {
            Connection = await _joinCommand.RunCommand(ctx, _player, Connection, Voice);
        }

        [Command("leave")]
        public async Task Leave(CommandContext ctx)
        {
            await _stopCommand.RunCommand(ctx, _player);
            await _leaveCommand.RunCommand(ctx, Connection);
            Connection = null;
        }
    }
}
