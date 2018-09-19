using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;
using TotallyNotABot.core;
using TotallyNotABot.src.commands;

namespace TotallyNotABot.commands
{
    class Play : BaseCommand
    {
        public Play(string name) : base(name)
        {
        }

        /// <summary>
        /// Validate the input given by the user
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="player"></param>
        /// <returns>the number passed by the user or -1 if it is invalid</returns>
        private async Task<string> CheckMessage(CommandContext ctx, Player player)
        {
            string[] msg = ctx.Message.Content.Split(" ");
            // Length of the message
            if (msg.Length <= 1) {
                await ctx.RespondAsync($"Please use the command !play [1-5] to select a song from the !search command");
                return "";
            }
            // Get the commands content
            int prefixSize = Settings.Prefix.Length;
            string command = ctx.Message.Content.Substring(prefixSize + "play".Length);
            
            return command;
        }

        public async Task RunCommand(CommandContext ctx, Player player)
        {
            bool added = false;
            string command = await this.CheckMessage(ctx, player);
            // The message is a valid number
            if (int.TryParse(command, out int number))
            {
                if (number >= 1 || number <= 5)
                {
                    if (number == -1) {
                        return;
                    }

                    added = player.Add(number - 1);
                }
                else
                {
                    await ctx.RespondAsync($"Please pass a valid number between 1 and 5");
                }
            }
            else
            {
                added = player.Add(command);
            }

            if (!added)
            {
                List<Song> list = await Search.DoSearch(command, player);
                added = player.Add(command);
                player.source.SearchList.Clear();
            }

            if (added)
            {
                player.Play();
                await ctx.RespondAsync(player.Current.ToString());
            }
            else
            {
                await ctx.RespondAsync($"The video \"{command}\" could not be found!");
            }
        }

        public override string Help()
        {
            return (DiscordFormat.DiscordString.Bold("Play: ") + "\nUse !play [1-5] to select a song from the !search command " + "\nUse !play [name of the song] to search and play a song");
        }
    }
}
