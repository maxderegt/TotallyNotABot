using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using TotallyNotABot.src.audio;
using VideoLibrary;
using YoutubeExplode;

namespace TotallyNotABot.src.commands
{
    class Commands
    {
        // Discord connection
        private static VoiceNextClient _voice;
        private static VoiceNextConnection _connection;
        private static DiscordClient _discord;

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

        public Commands()
        {
            _searchCommand = new Search();
            _startCommand = new Start();
            _playCommand = new Play();
            _stopCommand = new Stop();
            _joinCommand = new Join();
            _spamCommand = new Spam();
            _leaveCommand = new Leave();
        }

        public void Setdiscord(DiscordClient client, VoiceNextClient voice)
        {
            _discord = client;
            _voice = voice;
            _audio = new Audio(_connection);
        }

        [Command("search")]
        public async Task Search(CommandContext ctx)
        {
            await _searchCommand.RunCommand(ctx, _audio);
        }

        [Command("spam")]
        public async Task Spam(CommandContext ctx)
        {
            await _spamCommand.RunCommand(ctx, _discord);
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
            _connection = await _joinCommand.RunCommand(ctx, _audio, _connection, _voice);
        }

        [Command("leave")]
        public async Task Leave(CommandContext ctx)
        {
            await _stopCommand.RunCommand(ctx, _audio);
            await _leaveCommand.RunCommand(ctx, _connection);
        }
    }
}
