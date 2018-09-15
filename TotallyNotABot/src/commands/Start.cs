using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using TotallyNotABot.audio;

namespace TotallyNotABot.commands
{
    class Start
    {
        public void RunCommand(CommandContext ctx, Player player)
        {
            player.Play();
        }
    }
}
