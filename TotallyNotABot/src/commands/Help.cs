using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using TotallyNotABot.DiscordFormat;

namespace TotallyNotABot.src.commands
{
    class HelpCommando : BaseCommand
    {
        public HelpCommando(string name) : base(name)
        {
        }

        public async void RunCommand(CommandContext ctx, List<BaseCommand> list)
        {
            string[] msg = ctx.Message.Content.ToString().Split(' ');

            if (msg.Length <= 1)
            {
                StringBuilder builder = new StringBuilder(DiscordString.Bold("Commands:\n").Underline().ToString());

                foreach (BaseCommand command in list)
                {
                    builder.Append(command.Help() + "\n");
                }

                await ctx.RespondAsync(builder.ToString());
            }
            else
            {
                foreach (BaseCommand command in list)
                {
                    if (msg[1].Equals(command.name))
                    {
                        await ctx.RespondAsync(command.Help());
                    }
                }
            }
        }

        public override string Help()
        {
            return (DiscordString.Bold("Help: ") + "\nUse !help to get a list of how to use all the commands" +
                "\nUse !help [name of command] to get information on how to use that command");
        }
    }
}
