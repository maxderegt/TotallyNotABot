using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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
        private static Leave _leaveCommand;
        private static Next _nextCommand;
        private static PlayList _playListCommand;
        private static HelpCommando _helpCommand;

        // Other stuff
//        private static Audio _audio;
        private static Player _player;
        //list of all command (for the help command)
        private static List<BaseCommand> _commandList;

        public static void Init(DiscordClient client, VoiceNextClient voice)
        {
            Discord = client;
            Voice = voice;
            _player = new Player();

            // Commands
            _searchCommand = new Search("search");
            _startCommand = new Start("start");
            _playCommand = new Play("play");
            _stopCommand = new Stop("stop");
            _joinCommand = new Join("join");
            _leaveCommand = new Leave("leave");
            _nextCommand = new Next("next");
            _playListCommand = new PlayList("playlist");
            _helpCommand = new HelpCommando("help");
            _commandList = new List<BaseCommand>() { _joinCommand, _leaveCommand, _searchCommand, _playCommand, _nextCommand, _playListCommand, _stopCommand, _startCommand, _helpCommand};
        }

        internal static Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            _helpCommand.RunCommand(e.Context, _commandList);
            return Task.CompletedTask;
        }

        [Command("help")]
        public async Task Help(CommandContext ctx)
        {
            var embedbuilder = new DiscordEmbedBuilder();
            embedbuilder.Color = new DiscordColor("#4242f4");
            embedbuilder.Title = "embed test";
            embedbuilder.Description = "this is an description";
            await ctx.RespondAsync(embed: embedbuilder);
            _helpCommand.RunCommand(ctx, _commandList);
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
