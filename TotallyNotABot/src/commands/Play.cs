using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;

namespace TotallyNotABot.commands
{
    class Play
    {
        /// <summary>
        /// Validate the input given by the user
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="player"></param>
        /// <returns>the number passed by the user or -1 if it is invalid</returns>
        private async Task<int> CheckMessage(CommandContext ctx, Player player)
        {
            string[] msg = ctx.Message.Content.Split(" ");
            // Length of the message
            if (msg.Length <= 1) {
                await ctx.RespondAsync($"Please use the command !play [1-5] to select a song from the !search command");
                return -1;
            }
            string command = msg[1];
            // The search command has to have been called before
            if (!player.HasSearch()) {
                await ctx.RespondAsync($"Please use the command !search [name of a song] first");
                return -1;
            }
            // The message is a valid number
            if (int.TryParse(command, out int number)) {
                if (number >= 1 || number <= 5) {
                    return number;
                }
            }
            await ctx.RespondAsync($"Please pass a valid number between 1 and 5");
            return -1;
        }

        public async Task RunCommand(CommandContext ctx, Player player)
        {
            int number = await this.CheckMessage(ctx, player);
            if (number == -1) {
                return;
            }

            player.Add(number - 1);
            player.Play();
            await ctx.RespondAsync($"Added to queue");
            await ctx.RespondAsync(player.Current.ToString());
        }
    }
}
