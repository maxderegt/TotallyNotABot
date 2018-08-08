using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace TotallyNotABot.commands
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
                    ulong id = ulong.Parse(idstring);

                    DiscordUser user = await discord.GetUserAsync(id);
                    DiscordDmChannel channel = await discord.CreateDmAsync(user);

                    int.TryParse(msg[2], out int j);
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
