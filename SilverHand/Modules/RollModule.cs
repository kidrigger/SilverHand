using Discord.Commands;
using SilverHand.Core;
using System.Threading.Tasks;

namespace SilverHand.Modules
{
	public class RollModule : ModuleBase<SocketCommandContext>
	{
		[Command("r")]
		[Summary("Rolls a dice")]
		public Task RollDiceAsync([Remainder][Summary("The dice roll")] DiceRoll diceroll)
		{
			var nick = Context.Guild.GetUser(Context.Message.Author.Id).Nickname;
			long roll = DiceRoller.Roll(diceroll, out string rolllog);
			return ReplyAsync($"{rolllog}\n {nick}'s roll {diceroll} => **`{roll}`**");
		}
	}
}
