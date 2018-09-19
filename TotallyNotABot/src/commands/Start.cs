using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;
using TotallyNotABot.DiscordFormat;

namespace TotallyNotABot.src.commands
{
    class Start : BaseCommand
    {
        public Start(string name) : base(name)
        {
        }

        public void RunCommand(CommandContext ctx, Player player)
        {
            player.Play();
        }

        public override string Help()
        {
            return (DiscordString.Bold("Start: ") + "\nUse !start to start playing music when the bot has been stopped with the Stop command");
        }
    }
}
