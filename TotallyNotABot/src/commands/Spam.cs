using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using TotallyNotABot.src.audio;

namespace TotallyNotABot.src.commands
{
    class Spam
    {
        public async Task RunCommand(CommandContext ctx, DiscordClient discord)
        {
            string[] msg = ctx.Message.Content.Split(" ");
            if (msg.Length == 1)
            {
                await ctx.Message.RespondAsync($"Spam who? {ctx.User.Mention}");
            }
            else
            {
                if (msg[1].StartsWith("<@"))
                {
                    char[] remove = { '<', '@', '>', '!' };
                    string idstring = msg[1].Trim(remove);
                    ulong id = UInt64.Parse(idstring);

                    DiscordUser user = await discord.GetUserAsync(id);
                    DiscordDmChannel channel = await discord.CreateDmAsync(user);

                    int j = 1;
                    Int32.TryParse(msg[2], out j);
                    if (j < 6 || ctx.Member.IsOwner)
                        for (int i = 0; i < j; i++)
                        {
                            await channel.SendMessageAsync("Spam");
                        }
                    else
                        await ctx.Message.RespondAsync("NO");
                }
                else
                    await ctx.Message.RespondAsync($"Spam who? {ctx.User.Mention}");
            }
        }
    }
}
